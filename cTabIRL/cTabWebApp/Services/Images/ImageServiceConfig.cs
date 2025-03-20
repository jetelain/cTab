using System;

namespace cTabWebApp.Services.Images
{
    public class ImageServiceConfig
    {
        /// <summary>
        /// Maximum number of images (typical 720p image size is 300 KB, so 4.5 GB per default)
        /// </summary>
        public int MaxTotalImageCount { get; set; } = 15000;

        /// <summary>
        /// Maximum number of images per user session
        /// </summary>
        public int MaxSessionImageCount { get; set; } = 75;

        /// <summary>
        /// Duration to keep images
        /// </summary>
        public TimeSpan RetentionDuration { get; set; } = TimeSpan.FromHours(12);

        /// <summary>
        /// Directory to store images
        /// </summary>
        public string? StorageLocation { get; set; }

        public int MaxHeight { get; set; } = 720;

        public int MaxWidth { get; set; } = 1280; 

        public int MaxImageSizeInBytes { get; set; } = 1_048_576;
    }
}
