using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Services.Images
{
    public class ImageArchiveService : IImageArchiveService
    {
        private readonly IImageService _imageService;

        public ImageArchiveService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task GenerateArchiveAsync(Stream stream, List<PlayerTakenImage> images)
        {
            var num = 1;

            var worldName = images.Select(i => i.WorldName).Where(w => !string.IsNullOrEmpty(w)).FirstOrDefault() ?? "unknown";

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            var imageData = new Dictionary<string, string>();

            foreach (var stored in images)
            {
                imageData.Add($"{num}.jpeg", stored.Data ?? string.Empty);
                var entry = archive.CreateEntry($"{num}.jpeg", CompressionLevel.NoCompression);
                using (var entryStream = entry.Open())
                using (var imageStream = _imageService.OpenImage(stored))
                {
                    if (imageStream != null)
                    {
                        await imageStream.CopyToAsync(entryStream);
                    }
                }
                num++;
            }
            WriteMetadataFile(archive, worldName, imageData);
        }

        private void WriteMetadataFile(ZipArchive archive, string worldName, Dictionary<string, string> imageData)
        {
            var entry = archive.CreateEntry($"metadata.json", CompressionLevel.NoCompression);
            using var entryStream = entry.Open();
            JsonSerializer.Serialize(entryStream, new ImagesExportJson
            {
                WorldName = worldName,
                Data = imageData
            });
        }

        public async Task<ImportArchiveResult> ImportArchiveAsync(PlayerState player, Stream stream, IPAddress? remote)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

            var index = archive.GetEntry("metadata.json");
            if (index == null)
            {
                return new ImportArchiveResult("Missing 'metadata.json'");
            }
            ImagesExportJson? indexContent;
            using (var indexStream = index.Open())
            {
                indexContent = await JsonSerializer.DeserializeAsync<ImagesExportJson>(indexStream);
            }
            if (indexContent == null)
            {
                return new ImportArchiveResult("Invalid 'metadata.json'");
            }
            if (indexContent.WorldName != player.LastMission?.WorldName)
            {
                return new ImportArchiveResult($"Map mismatch: archive is '{indexContent.WorldName}' but session is '{player.LastMission?.WorldName}'");
            }
            var result = new ImportArchiveResult();
            foreach (var entry in indexContent.Data)
            {
                var imageEntry = archive.GetEntry(entry.Key);
                if (imageEntry != null)
                {
                    var bytes = new MemoryStream();
                    using (var entryStream = imageEntry.Open())
                    {
                        await entryStream.CopyToAsync(bytes);
                    }
                    var playerImage = await _imageService.SaveImageAsync(player, bytes.ToArray(), entry.Value, remote);
                    if (playerImage != null)
                    {
                        result.Images.Add(playerImage);
                    }
                }
            }
            return result;
        }
    }
}
