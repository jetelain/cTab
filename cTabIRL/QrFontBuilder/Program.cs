using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace QrFontBuilder
{
    class Program
    {
        const int charsPerLine = 16;
        const int charPixelsWidth = 2;
        const int charPixelsHeight = 3;
        const string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

        static void Main(string[] args)
        {

            string imageToPaa;
            using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Bohemia Interactive\ImageToPAA"))
            {
                imageToPaa = key.GetValue("tool") as string;
            }

            Generate("qrfont6", 4, imageToPaa);
            Generate("qrfont7", 4, imageToPaa);
            Generate("qrfont8", 5, imageToPaa);
            Generate("qrfont9", 5, imageToPaa);
            Generate("qrfont10", 6, imageToPaa);
            Generate("qrfont11", 6, imageToPaa);
            Generate("qrfont12", 7, imageToPaa);
            Generate("qrfont13", 7, imageToPaa);
            Generate("qrfont14", 8, imageToPaa);
            Generate("qrfont15", 8, imageToPaa);
            Generate("qrfont16", 9, imageToPaa);
            Generate("qrfont17", 9, imageToPaa);
            Generate("qrfont18", 10, imageToPaa);
            Generate("qrfont19", 10, imageToPaa);
            Generate("qrfont20", 11, imageToPaa);
            Generate("qrfont21", 11, imageToPaa);
            Generate("qrfont22", 12, imageToPaa);
            Generate("qrfont23", 12, imageToPaa);
            Generate("qrfont24", 13, imageToPaa);
            Generate("qrfont25", 13, imageToPaa);
            Generate("qrfont26", 14, imageToPaa);
            Generate("qrfont27", 14, imageToPaa);
            Generate("qrfont28", 15, imageToPaa);
            Generate("qrfont29", 15, imageToPaa);
        }

        private static void Generate(string name, int pixelSize, string imageToPaa)
        {
            using (var a = new BinaryWriter(new FileStream(name + ".fxy", FileMode.OpenOrCreate, FileAccess.Write)))
            {
                for (int c = 0x20; c < 0x100; ++c)
                {
                    var value = alpha.IndexOf((char)c);
                    var x = 0;
                    var y = 0;
                    if (value != -1)
                    {
                        x = value % charsPerLine;
                        y = value / charsPerLine;
                    }

                    a.Write((ushort)(c - 0x20));
                    a.Write((ushort)1);
                    a.Write((ushort)(x * charPixelsWidth * pixelSize));
                    a.Write((ushort)(y * charPixelsHeight * pixelSize));
                    a.Write((ushort)(charPixelsWidth * pixelSize));
                    a.Write((ushort)(charPixelsHeight * pixelSize));
                }
            }

            var size = 128;
            while (charPixelsWidth * charsPerLine * pixelSize > size) size *= 2;

            using (var image = new Image<Rgba32>(null, size, size, new Rgba32(0, 0, 0, 0)))
            {

                var x = 0;
                var y = 0;
                for (int i = 0; i < alpha.Length; ++i)
                {
                    DrawPixel(image, x + 0, y + 0, i & 0x01, pixelSize);
                    DrawPixel(image, x + 1, y + 0, i & 0x02, pixelSize);
                    DrawPixel(image, x + 0, y + 1, i & 0x04, pixelSize);
                    DrawPixel(image, x + 1, y + 1, i & 0x08, pixelSize);
                    DrawPixel(image, x + 0, y + 2, i & 0x10, pixelSize);
                    DrawPixel(image, x + 1, y + 2, i & 0x20, pixelSize);

                    x += charPixelsWidth;
                    if (x >= (charPixelsWidth * charsPerLine))
                    {
                        y += charPixelsHeight;
                        x = 0;
                    }

                }
                image.SaveAsPng(name+"-01.png");
            }

            Process process = new Process();
            process.StartInfo.FileName = imageToPaa;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(imageToPaa);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.ArgumentList.Add(Path.GetFullPath(name+"-01.png"));
            process.Start();
            process.WaitForExit();
        }

        static void DrawPixel(Image<Rgba32> image, int x, int y, int value, int pixelSize)
        {
            if(value!=0)
            {
                var white = new Rgba32(255, 255, 255, 255);
                for(int i =0;i< pixelSize; ++i)
                {
                    for (int j = 0; j < pixelSize; ++j)
                    {
                        image[x * pixelSize + i, y * pixelSize + j] = white;
                    }
                }
            }
        }
    }
}
