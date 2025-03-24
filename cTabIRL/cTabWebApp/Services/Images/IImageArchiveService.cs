using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Services.Images
{
    public interface IImageArchiveService
    {
        Task<ImportArchiveResult> ImportArchiveAsync(PlayerState player, Stream stream, IPAddress? remote);

        Task GenerateArchiveAsync(Stream stream, List<PlayerTakenImage> images);

    }
}
