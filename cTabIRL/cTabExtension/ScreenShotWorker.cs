using System.Net.Http.Headers;

namespace cTabExtension
{
    internal class ScreenShotWorker
    {
        private static readonly HttpClient webClient = new HttpClient() { DefaultRequestHeaders = { { "Extension", Worker.ExtensionHeader } } };

        private readonly ScreenShotOptions options;

        public ScreenShotWorker(ScreenShotOptions options)
        {
            this.options = options;
            options.MaxHeight = Math.Max(720, options.MaxHeight);
            options.MaxWidth = Math.Max(1280, options.MaxWidth);
        }

        /// <summary>
        /// Tells the mod that the screenshot functionality is available
        /// </summary>
        internal void Callback()
        {
            Extension.Callback("ScreenShotEnabled", $"[{options.MaxWidth},{options.MaxHeight}]");
        }

        /// <summary>
        /// Takes a screenshot and sends it to the web application
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal async Task TakeScreenShotInternal(string?[] args)
        {
            var endpoint = options.Endpoint;
            if (string.IsNullOrEmpty(endpoint))
            {
                return;
            }

            var bytes = ScreenShotHelper.TakeScreenShot(options.MaxWidth,options.MaxHeight);

            var data = GetData(args);
            var content = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            content.Add(byteArrayContent, "file", "screenshot.jpg");
            content.Add(new StringContent(options.Token ?? string.Empty), "token");
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
