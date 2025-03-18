using System;

namespace cTabWebApp.Services.Images
{
    public class ImageServiceConfig
    {
        public int MaxTotalImageCount { get; set; } = 15000; // Maximum number of images (typical image size is 300 KB, so 4.5 GB per default)
        public int MaxSessionImageCount { get; set; } = 75; // Maximum number of images per user session
        public TimeSpan RetentionDuration { get; set; } = TimeSpan.FromHours(12); // Duration to keep images
        public string? StorageLocation { get; set; } // Directory to store images

        public int MaxHeight { get; set; } = 720;

        public int MaxWidth { get; set; } = 1280;

    }
}
