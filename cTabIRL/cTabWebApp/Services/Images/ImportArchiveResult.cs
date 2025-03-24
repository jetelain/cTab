using System.Collections.Generic;

#nullable enable

namespace cTabWebApp.Services.Images
{
    public class ImportArchiveResult
    {
        public ImportArchiveResult(string error)
        {
            Error = error;
        }

        public ImportArchiveResult()
        {
        }

        public string? Error { get; set; }

        public List<PlayerTakenImage> Images { get; } = new List<PlayerTakenImage>();
    }
}
