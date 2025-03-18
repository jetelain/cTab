using System.IO;
using System.Net;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Services.Images
{
    public interface IImageService
    {
        Task<PlayerTakenImage?> SaveImageAsync(PlayerState player, byte[] image, string data, IPAddress? remote);

        PlayerTakenImage? GetImage(string token);

        Stream? OpenImage(PlayerTakenImage id);
    }
}
