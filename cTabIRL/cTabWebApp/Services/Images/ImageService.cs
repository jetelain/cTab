using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

#nullable enable

namespace cTabWebApp.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly ConcurrentDictionary<string, PlayerTakenImage> images = new ConcurrentDictionary<string, PlayerTakenImage>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private readonly string imageDirectory;
        private readonly int maxTotalImageCount;
        private readonly int maxSessionImageCount;
        private readonly TimeSpan retentionDuration;
        private readonly int maxHeight;
        private readonly int maxWidth;

        public ImageService(ImageServiceConfig config)
        {
            maxTotalImageCount = config.MaxTotalImageCount; // Maximum number of images (typical image size is 300 KB, so 4.5 GB per default)
            maxSessionImageCount = config.MaxSessionImageCount; // Maximum number of images per user session
            retentionDuration = config.RetentionDuration; // Duration to keep images
            imageDirectory = config.StorageLocation ?? Path.Combine(Path.GetTempPath(), "cTabWebApp", "PlayerTakenImages");
            maxHeight = config.MaxHeight;
            maxWidth = config.MaxWidth;

            Directory.CreateDirectory(imageDirectory);

            // Restore images (if server was restarted during a game session)
            RestoreImages();
        }

        private bool IsExpired(PlayerTakenImage entry)
        {
            return entry.ExpiresUtc < DateTime.UtcNow;
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
                    if (entry != null && entry.Token == token)
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
            if (images.TryGetValue(token, out var entry) && !IsExpired(entry))
            {
                return entry;
            }
            return null;
        }

        private List<PlayerTakenImage> TakeExpiredImagesLocked()
        {
            // Assume already locked
            var expired = images.Values.Where(IsExpired).ToList();
            RemoveEntriesLocked(expired);
            return expired;
        }

        private void RemoveEntriesLocked(List<PlayerTakenImage> expired)
        {
            // Assume already locked
            foreach (var entry in expired)
            {
                images.TryRemove(entry.Token, out _);

                if (entry.Player != null)
                {
                    entry.Player.Images.Remove(entry);
                }
            }
        }

        public async Task CleanUp() // TODO: Should be called periodically
        {
            List<PlayerTakenImage> toremove;
            await semaphore.WaitAsync();
            try
            {
                toremove = TakeExpiredImagesLocked();
            }
            finally
            {
                semaphore.Release();
            }
            DeleteFiles(toremove);
        }

        private void DeleteFiles(List<PlayerTakenImage> toremove)
        {
            foreach (var entry in toremove)
            {
                DeleteFiles(entry);
            }
        }

        private async Task<PlayerTakenImage?> AllocatePlayerTakenImage(PlayerState player, string data, IPAddress? remote)
        {
            await semaphore.WaitAsync();
            try
            {
                if (!TryAcquireQuotaLocked(player))
                {
                    return null;
                }
                var token = AllocateTokenLocked(player);
                var entry = new PlayerTakenImage() 
                {
                    TimestampUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow + retentionDuration, 
                    Token = token, 
                    OwnerSteamId = player.SteamId,
                    Data = data, 
                    WorldName = player.LastMission?.WorldName, 
                    Remote = remote?.ToString(), 
                    Player = player 
                };
                images.TryAdd(token, entry);
                player.Images.Add(entry);
                return entry;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private bool TryAcquireQuotaLocked(PlayerState player)
        {
            // Assume already locked

            // Check if the user has exceeded their quota
            if (player.Images.Count > maxSessionImageCount)
            {
                // Remove the oldest images
                var toRemoveFromUser = player.Images.OrderBy(img => img.TimestampUtc).Take(player.Images.Count - maxTotalImageCount + 1).ToList();
                RemoveEntriesLocked(toRemoveFromUser);
                Task.Run(() => DeleteFiles(toRemoveFromUser));
                return true;
            }

            // Check if the total image count has been exceeded
            if (images.Count >= maxTotalImageCount)
            {
                // Try to remove expired images
                var toremove = TakeExpiredImagesLocked();
                if (toremove.Count > 0)
                {
                    // Remove asynchronously
                    Task.Run(() => DeleteFiles(toremove));
                    return true;
                }
                return false;
            }
            return true;
        }

        private string AllocateTokenLocked(PlayerState player)
        {
            string token;
            do
            {
                token = player.SteamId + "x" + PlayerStateService.ToBase64Url(RandomNumberGenerator.GetBytes(24));
            }
            while (images.ContainsKey(token));
            return token;
        }

        public async Task<PlayerTakenImage?> SaveImageAsync(PlayerState player, byte[] image, string data, IPAddress? remote)
        {
            var identify = await Image.IdentifyAsync(new MemoryStream(image));
            if (!IsValidImage(identify))
            {
                return null;
            }

            var id = await AllocatePlayerTakenImage(player, data, remote);
            if (id == null)
            {
                return null;
            }

            id.Size = image.Length;

            await File.WriteAllBytesAsync(GetImagePath(id), image);

            await File.WriteAllTextAsync(GetJsonPath(id), JsonSerializer.Serialize(id));

            return id;
        }

        private bool IsValidImage(ImageInfo identify)
        {
            // Check if the image is a JPEG and has the correct dimensions
            if (identify.Height > maxHeight || identify.Width > maxWidth)
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
