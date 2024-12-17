using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

#nullable enable

namespace cTabWebApp.Services
{
    public class ImageService : IImageService
    {
        private readonly ConcurrentDictionary<string, PlayerTakenImage> images = new ConcurrentDictionary<string, PlayerTakenImage>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        internal const int TargetHeight = 720;
        internal const int TargetWidth = 1280;

        private string imageDirectory = Path.Combine(Path.GetTempPath(), "cTabWebApp", "PlayerTakenImages");

        public ImageService()
        {
            Directory.CreateDirectory(imageDirectory);

            // Restore images (if server was restarted during a game session)
            RestoreImages();
        }

        private void RestoreImages()
        {
            foreach (var file in Directory.EnumerateFiles(imageDirectory, "*.json"))
            {
                var token = Path.GetFileNameWithoutExtension(file);
                var json = File.ReadAllText(file);
                try
                {
                    var entry = JsonSerializer.Deserialize<PlayerTakenImage>(json);
                    if (entry != null && !entry.IsExpired && entry.Token == token)
                    {
                        images.TryAdd(entry.Token, entry);
                    }
                    else
                    {
                        DeleteFiles(entry ?? new PlayerTakenImage() { Token = token });
                    }
                }
                catch (JsonException)
                {
                    File.Delete(file);
                    DeleteFiles(new PlayerTakenImage() { Token = token });
                }
            }
        }

        public PlayerTakenImage? GetImage(string token)
        {
            return images.TryGetValue(token, out var entry) ? entry : null;
        }

        private List<PlayerTakenImage> TakeExpiredImages()
        {
            var expired = images.Values.Where(i => i.IsExpired).ToList();
            foreach (var entry in expired)
            {
                images.TryRemove(entry.Token, out _);
            }
            return expired;
        }

        public async Task CleanUp() // TODO: Should be called periodically
        {
            List<PlayerTakenImage> toremove;
            await semaphore.WaitAsync();
            try
            {
                toremove = TakeExpiredImages();
            }
            finally
            {
                semaphore.Release();
            }
            foreach (var entry in toremove)
            {
                DeleteFiles(entry);
            }
        }

        private async Task<PlayerTakenImage> AllocatePlayerTakenImage(PlayerState player, string data)
        {
            await semaphore.WaitAsync();
            try
            {
                var token = AllocateToken(player);
                var entry = new PlayerTakenImage() { Token = token, OwnerSteamId = player.SteamId, Data = data, WorldName = player.LastMission?.WorldName };
                images.TryAdd(token, entry);
                return entry;
                // TODO: Should enforce a quota here (per user and globally)
            }
            finally
            {
                semaphore.Release();
            }
        }

        private string AllocateToken(PlayerState player)
        {
            string token;
            do
            {
                token = player.SteamId + "x" + PlayerStateService.ToBase64Url(RandomNumberGenerator.GetBytes(24));
            }
            while (images.ContainsKey(token));
            return token;
        }

        public async Task<PlayerTakenImage?> SaveImageAsync(PlayerState player, byte[] image, string data)
        {
            var identify = await Image.IdentifyAsync(new MemoryStream(image));
            if (!IsValidImage(identify))
            {
                return null;
            }
            var id = await AllocatePlayerTakenImage(player, data);

            await File.WriteAllBytesAsync(GetImagePath(id), image);

            await File.WriteAllTextAsync(GetJsonPath(id), JsonSerializer.Serialize(id));

            return id;
        }

        private static bool IsValidImage(ImageInfo identify)
        {
            // Check if the image is a JPEG and has the correct dimensions
            if (identify.Height > TargetHeight || identify.Width > TargetWidth)
            {
                return false;
            }
            if (!(identify.Metadata.DecodedImageFormat is JpegFormat))
            {
                return false;
            }
            // Ensure that no metadata is present
            if (identify.Metadata.ExifProfile != null
                || identify.Metadata.IccProfile != null
                || identify.Metadata.IptcProfile != null
                || identify.Metadata.CicpProfile != null
                || identify.Metadata.XmpProfile != null)
            {
                return false;
            }
            return true;
        }

        public Stream? OpenImage(PlayerTakenImage id)
        {
            var imagePath = GetImagePath(id);
            if (!File.Exists(imagePath))
            {
                return null;
            }
            return File.OpenRead(imagePath);
        }

        private string GetImagePath(PlayerTakenImage id)
        {
            return Path.Combine(imageDirectory, $"{id.Token}.full.jpeg");
        }

        private string GetJsonPath(PlayerTakenImage id)
        {
            return Path.Combine(imageDirectory, $"{id.Token}.json");
        }

        private void DeleteFiles(PlayerTakenImage id)
        {
            var json = GetJsonPath(id);
            if (File.Exists(json))
            {
                File.Delete(json);
            }

            var image = GetImagePath(id);
            if (File.Exists(image))
            {
                File.Delete(image);
            }
        }

    }
}
