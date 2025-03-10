using System.Net.Http.Headers;

namespace cTabExtension
{
    internal class ScreenShotWorker
    {
        private static readonly HttpClient webClient = new HttpClient() { DefaultRequestHeaders = { { "Extension", Worker.ExtensionHeader } } };

        private readonly string screenShotEndpoint;
        private readonly string screenShotToken;

        public ScreenShotWorker(string screenShotEndpoint, string screenShotToken)
        {
            this.screenShotEndpoint = screenShotEndpoint;
            this.screenShotToken = screenShotToken;
        }

        internal async Task TakeScreenShotInternal(string?[] args)
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
    }
}
