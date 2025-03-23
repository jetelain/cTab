using System.Collections.Generic;
#nullable enable

namespace cTabWebApp.Services.Images
{
    public class ImagesExportJson
    {
        public required string WorldName { get; set; }

        public required Dictionary<string, string> Data { get; set; }
    }
}
