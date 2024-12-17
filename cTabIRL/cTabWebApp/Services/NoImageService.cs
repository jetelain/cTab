using System.IO;
using System.Threading.Tasks;

namespace cTabWebApp.Services
{
    public class NoImageService : IImageService
    {
        public PlayerTakenImage GetImage(string token)
        {
            return null;
        }

        public Stream OpenImage(PlayerTakenImage id)
        {
            return null;
        }

        public Task<PlayerTakenImage> SaveImageAsync(PlayerState player, byte[] image, string data)
        {
            return Task.FromResult<PlayerTakenImage>(null);
        }
    }
}
