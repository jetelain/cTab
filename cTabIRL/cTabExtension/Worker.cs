using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace cTabExtension
{
    public static class Worker
    {
        private const string ExtensionHeader = "cTabExtension/1.2";

        private static readonly HttpClient webClient = new HttpClient() { DefaultRequestHeaders = { { "Extension", ExtensionHeader } } };
        private static Task<HubConnection?>? serverConnection;
        private static CancellationTokenSource? cancellationTokenSource;
        private static List<Tuple<string, Func<HubConnection, Task>>> replay = new List<Tuple<string, Func<HubConnection, Task>>>();
        private static string? screenShotEndpoint;
        private static string? screenShotToken;

        public static void Message(string function,string?[] args)
        {
            if (function == "Connect")
            {
                if (args.Length != 4)
                {
                    return;
                }
                if (serverConnection != null && cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    Send("Disconnect", async srv => await srv.DisposeAsync());
                }
                cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                serverConnection = Task.Factory.StartNew(() => Connect(args, token), token).Unwrap();
            }
            if (serverConnection != null)
            {
                Task.Factory.StartNew(() => Send(function, args));
            }
        }

        private static void Send(string function, string?[] args)
        {
            switch (function)
            {
                case "ScreenShot":
                    TakeScreenShot(args);
                    break;
                // Frequent messages, can be dropped without impact
                case "UpdatePosition":
                case "UpdateMarkersPosition":
                    if (!serverConnection?.IsCompleted ?? false) // Those messages can be dropped if connection is still in progress
                    {
                        return;
                    }
                    Send(function, srv => srv.SendAsync("Arma" + function, new ArmaMessage() { Timestamp = DateTime.UtcNow, Args = args }));
                    break;
                // Mission state messages, should wait server to process them, to ensure server state is correct
                case "StartMission":
                case "EndMission":
                    Send(function, srv => srv.InvokeAsync("Arma" + function, new ArmaMessage() { Timestamp = DateTime.UtcNow, Args = args }), true);
                    break;
                // Other state messages :
                // if starts with Action, it's a realtime message (does not need replay and should be dispatched quickly)
                // Otherwise the last one has all relevant data (keep and replay last one if an issue occurs) 
                default:
                    Send(function, srv => srv.SendAsync("Arma" + function, new ArmaMessage() { Timestamp = DateTime.UtcNow, Args = args }),
                        !function.StartsWith("Action", StringComparison.Ordinal));
                    break;
            }
        }

        private static void TakeScreenShot(string?[] args)
        {
            serverConnection?.ContinueWith(async srv =>
            {
                try
                {
                    await TakeScreenShotInternal(args);
                }
                catch (Exception e)
                {
                    Extension.ErrorMessage($"TakeScreenShot failed with {e.GetType().Name} {e.Message}.");
                    ReportInner(e);
                    await Extension.Callback("ScreenShotFailed", "");
                }
            });
        }

        private static async Task TakeScreenShotInternal(string?[] args)
        {
            var endpoint = screenShotEndpoint;
            if (string.IsNullOrEmpty(endpoint))
            {
                return;
            }

            var bytes = ScreenShotHelper.TakeScreenShot();

            var data = GetData(args);
            var content = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            content.Add(byteArrayContent, "file", "screenshot.jpg");
            content.Add(new StringContent(screenShotToken ?? string.Empty), "token");
            content.Add(new StringContent(data), "data");

            var response = await webClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                await Extension.Callback("ScreenShotStored", $"['{result}',{data}]");
            }
            else
            {
                await Extension.Callback("ScreenShotFailed", "");
            }
        }

        private static string GetData(string?[] args)
        {
            var data = args.Length > 0 ? args[0] : null;
            if (string.IsNullOrEmpty(data))
            {
                data = "[]";
            }
            return data;
        }

        private static async Task<HubConnection?> Connect(string?[] args, CancellationToken token)
        {
            lock (replay)
            {
                replay.Clear();
            }
            try
            {
                return await ConnectToServer(ArmaString(args[0]), ArmaString(args[1]), ArmaString(args[2]), ArmaString(args[3]), token).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                Extension.ErrorMessage($"Connect failed with {e.GetType().Name} {e.Message}.");
                ReportInner(e);
                return null;
            }
        }

        private static void Send(string name, Func<HubConnection, Task> action, bool needsReplayLast = false)
        {
            serverConnection?.ContinueWith(async srv =>
            {
                if (srv.Result == null || srv.Result.State == HubConnectionState.Disconnected)
                {
                    return;
                }
                if (srv.Result.State == HubConnectionState.Reconnecting)
                {
                    if (needsReplayLast)
                    {
                        AddToReplay(name, action);
                    }
                    return;
                }
                try
                {
                    Extension.DebugMessage($"Send {name}...");
                    await action(srv.Result);
                    Extension.DebugMessage($"{name} done.");
                    if (needsReplayLast)
                    {
                        AddToReplay(name, action);
                    }
                }
                catch(AggregateException ae)
                {
                    foreach(var e in ae.InnerExceptions)
                    {
                        Extension.ErrorMessage($"{name} failed with {e.GetType().Name} {e.Message}.");
                        ReportInner(e);
                    }
                }
                catch (Exception e)
                {
                    Extension.ErrorMessage($"{name} failed with {e.GetType().Name} {e.Message}.");
                    ReportInner(e);
                }
            }).Unwrap();
        }

        private static void AddToReplay(string name, Func<HubConnection, Task> action)
        {
            lock (replay)
            {
                if (name == "EndMission") { replay.RemoveAll(e => e.Item1 == "StartMission"); }
                if (name == "StartMission") { replay.RemoveAll(e => e.Item1 == "EndMission"); }
                replay.RemoveAll(e => e.Item1 == name);
                replay.Add(new Tuple<string, Func<HubConnection, Task>>(name, action));
            }
        }

        private static string ArmaString(string? str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            if (str.StartsWith("\"") && str.EndsWith("\""))
            {
                return str.Substring(1, str.Length - 2);
            }
            return str;
        }

        private static void EnableScreenShot(string endpoint, string token)
        {
            screenShotToken = token;
            screenShotEndpoint = endpoint;
            Extension.Callback("ScreenShotEnabled", "");
        }

        private static async Task<HubConnection?> ConnectToServer(string server, string steamId, string name, string key, CancellationToken token)
        {
            var uri = new Uri(server);

            Extension.DebugMessage($"server={server}, steamId={steamId}, name={name}, key={key}, hostname={uri.DnsSafeHost}");

            var connection = new HubConnectionBuilder()
                .AddJsonProtocol(c => c.PayloadSerializerOptions.TypeInfoResolver = JsonContext.Default)
                .WithUrl(uri, options =>
                {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                    options.Headers.Add("Extension", ExtensionHeader);
                })
                .WithAutomaticReconnect()
                .Build();

            connection.On<string, string>("Callback", Extension.Callback);
            connection.On<string, string>("ScreenShotEnabled", EnableScreenShot);

            connection.Reconnecting += async _ => {
                Extension.DebugMessage($"Reconnecting...");
                await Extension.Callback("Reconnecting", "");
            };
            connection.Reconnected += async _ => { 
                Extension.DebugMessage($"Reconnected !");  
                await SayHello(steamId, name, HashKeyForHost(key, uri.DnsSafeHost), connection);
                foreach(var send in replay)
                {
                    await send.Item2(connection);
                }
            };
            connection.Closed += async e => {
                if (e != null)
                {
                    Extension.ErrorMessage($"Closed with {e.GetType().Name} {e.Message}.");
                    ReportInner(e);
                    await Extension.Callback("Disconnected", "");
                }
            };

            while (!token.IsCancellationRequested)
            {
                try
                {
                    Extension.DebugMessage($"Connecting...");
                    await connection.StartAsync(token);
                    Extension.DebugMessage($"Connected.");

                    await SayHello(steamId, name, HashKeyForHost(key, uri.DnsSafeHost), connection);
                    return connection;
                }
                catch(Exception e)
                {
                    Extension.ErrorMessage($"Connection failed with {e.GetType().Name} {e.Message}.");
                    ReportInner(e);
                    if (!token.IsCancellationRequested)
                    {
                        await Task.Delay(5000);
                    }
                }
            }
            return null;
        }

        private static void ReportInner(Exception e)
        {
            if (e.InnerException != null)
            {
                Extension.ErrorMessage($"+ {e.InnerException.GetType().Name} {e.InnerException.Message}.");
                ReportInner(e.InnerException);
            }
        }

        private static async Task SayHello(string steamId, string name, string key, HubConnection connection)
        {
            Extension.DebugMessage($"Invoke Hello...");
            await connection.InvokeAsync("ArmaHello", new ArmaHelloMessage() { Timestamp = DateTime.UtcNow, SteamId = steamId, PlayerName = name, Key = key });
            Extension.DebugMessage($"Hello done.");
        }

        private static string HashKeyForHost(string key, string hostname)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: key,
                            salt: Encoding.UTF8.GetBytes(hostname),
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));
        }
    }
}
