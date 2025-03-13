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

        public double[]? Location { get; set; }

        public string? ImageUri { get; set; }

        public bool ImageProject { get; set; }

        public double ImageDirection { get; set; }

        public double[]? ImageSize { get; set; }

        public double[][]? ImageArea { get; set; }

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