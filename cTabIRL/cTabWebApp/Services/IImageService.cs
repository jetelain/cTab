using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Services
{
    public interface IImageService
    {
        Task<PlayerTakenImage?> SaveImageAsync(PlayerState player, byte[] image, string data);

        PlayerTakenImage? GetImage(string token);

        Stream? OpenImage(PlayerTakenImage id);
    }
}
