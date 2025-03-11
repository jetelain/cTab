using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace cTabExtension
{
    internal static class ScreenShotHelper
    {
        internal static byte[] TakeScreenShot(int targetWidth, int targetHeight)
        {
            GetWindowRect(Process.GetCurrentProcess().MainWindowHandle, out RECT lpRect);
            var screenH = lpRect.Bottom;
            var screenW = lpRect.Right;

            var shotWidth = Math.Min(screenW, targetWidth * screenH / targetHeight);
            using var bitmap = new Bitmap(shotWidth, screenH);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point((screenW - bitmap.Width) / 2, 0), Point.Empty, new Size(bitmap.Width, bitmap.Height));
            }

            using var resized = ResizeBitmap(bitmap, bitmap.Width * targetHeight / bitmap.Height, targetHeight);

            using var ms = new MemoryStream();
            SaveJpegWithQuality(resized, ms, 95);
            return ms.ToArray();
        }

        internal static Bitmap ResizeBitmap(Bitmap originalBitmap, int newWidth, int newHeight)
        {
            var resizedBitmap = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(resizedBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalBitmap, 0, 0, newWidth, newHeight);
            }
            return resizedBitmap;
        }

        internal static void SaveJpegWithQuality(Bitmap bitmap, Stream stream, long quality)
        {
            var jpegCodec = GetEncoderInfo("image/jpeg");
            if (jpegCodec == null)
            {
                throw new Exception("JPEG codec not found");
            }
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            bitmap.Save(stream, jpegCodec, encoderParams);
        }

        private static ImageCodecInfo? GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == mimeType);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(nint hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
    }
}
