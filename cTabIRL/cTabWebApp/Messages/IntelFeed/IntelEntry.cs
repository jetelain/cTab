using System;
using System.Linq;
using Arma3TacMapLibrary.Arma3;
#nullable enable

namespace cTabWebApp.Messages.IntelFeed
{
    public class IntelEntry
    {
        public string? Id { get; set; }

        public DateTime DateTime { get; set; }

        /// <summary>
        /// Center of image in world coordinates [x,y,z]
        /// </summary>
        public double[]? Location { get; set; }

        public string? ImageUri { get; set; }

        public bool ImageProject { get; set; }

        /// <summary>
        /// Direction of image in world coordinates (degrees, north)
        /// </summary>
        public double ImageDirection { get; set; }

        /// <summary>
        /// Size of image in world coordinates (meters) [width, height]
        /// </summary>
        public double[]? ImageSize { get; set; }

        /// <summary>
        /// Corners of image in world coordinates
        /// [worldTopLeft, worldTopRight, worldBottomRight, worldBottomLeft]
        /// with each corner being [x,y,z]
        /// </summary>
        public double[][]? ImageArea { get; set; }

        /// <summary>
        /// Camera position in world coordinates [x,y,z]
        /// </summary>
        public double[]? ImageCamera { get; set; }

        internal static IntelEntry? Create(string entry)
        {
            var data = ArmaSerializer.ParseMixedArray(entry);
            if (data.Length < 5)
            {
                return null;
            }
            var id = Convert.ToString(data[0]);
            var location = ArmaConverter.ToDoubleArray(data[2]);
            var dateTime = ArmaConverter.ToDateTime(data[3]);
            var imageData = data[4] as object[];
            if (imageData == null || imageData.Length < 5)
            {
                return null;
            }
            var imageUri = imageData[0] as string ?? string.Empty;
            var imageCanDisplayOnMap = imageData[1] as bool? ?? false;
            var imageDirection = Convert.ToDouble(imageData[2]);
            var imageSize = ArmaConverter.ToDoubleArray(imageData[3]);
            var imageArea = (imageData[4] as object[])?.Select(ArmaConverter.ToDoubleArray)?.ToArray();
            var imageCamera = ArmaConverter.ToDoubleArray(imageData[5]);
            return new IntelEntry()
            {
                Id = id,
                Location = location,
                DateTime = dateTime,
                ImageUri = imageUri,
                ImageProject = imageCanDisplayOnMap,
                ImageDirection = imageDirection,
                ImageSize = imageSize,
                ImageArea = imageArea,
                ImageCamera = imageCamera
            };
        }
    }
}