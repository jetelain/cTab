using System.Diagnostics;
using App6dGenerator;
using Pmad.ProgressTracking;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LineFontsGenerator
{
    internal class Program
    {
        private static char[] Chars = Enumerable.Range(0x20, 0x7F - 0x20).Select(x => (char)x).Concat(['±']).ToArray();

        static async Task Main(string[] args)
        {
            await Generate("TahomaBLineOne8", 10, 1, 256);
            await Generate("TahomaBLineTwo8", 10, 2, 256);
            await Generate("TahomaBLineThree8", 10, 3, 256);

            await Generate("TahomaBLineOne16", 20, 1, 512);
            await Generate("TahomaBLineTwo16", 20, 2, 512);
            await Generate("TahomaBLineThree16", 20, 3, 512);

            await Generate("TahomaBLineOne32", 40, 1, 1024);
            await Generate("TahomaBLineTwo32", 40, 2, 1024);
            await Generate("TahomaBLineThree32", 40, 3, 1024);
        }

        private record CharBox(char Char, int X, int Y, int Width, int Height);

        private static async Task Generate(string name, float realFontSize, int lineNumber, int imageSize)
        {
            var font = SystemFonts.CreateFont("Tahoma", realFontSize, FontStyle.Bold);
            var options = new TextOptions(font);

            var normalLineHeight = (int)Math.Round(TextMeasurer.MeasureSize(new string(Chars), options).Height)+2;
            var fontLineHeight = normalLineHeight * 3;

            var boxes = new List<CharBox>();
            var offset = (lineNumber - 1) * normalLineHeight;

            var margin = (int)(realFontSize / 10 * 2);

            using (var image = new Image<Rgba32>(imageSize, imageSize, new Rgba32(0, 0, 0, 0)))
            {
                image.Mutate(ctx =>
                {
                    var point = new Point(margin, margin);

                    foreach (var c in Chars)
                    {
                        var measure = TextMeasurer.MeasureSize(c.ToString(), options);
                        var charSize = new Size((int)Math.Ceiling(measure.Width) + 2, (int)Math.Ceiling(measure.Height) + 2);
                        if (point.X + charSize.Width > image.Width)
                        {
                            point.X = margin;
                            point.Y += fontLineHeight + (margin * 2);
                        }
                        ctx.DrawText(c.ToString(), font, Color.White, point + new PointF(1, offset+1));
                        boxes.Add(new CharBox(c, point.X, point.Y, charSize.Width, fontLineHeight));
                        point.X += charSize.Width + (margin * 2);
                    }

                });
                await image.SaveAsPngAsync(name + "-01.png");
            }

            using (var a = new BinaryWriter(new FileStream(name + ".fxy", FileMode.OpenOrCreate, FileAccess.Write)))
            {
                for (int c = 0x20; c < 0x100; ++c)
                {
                    var box = boxes.FirstOrDefault(i => i.Char == (char)c) ?? boxes.First();
                    a.Write((ushort)(c - 0x20));
                    a.Write((ushort)1);
                    a.Write((ushort)(box.X));
                    a.Write((ushort)(box.Y));
                    a.Write((ushort)(box.Width));
                    a.Write((ushort)(box.Height));
                }
            }

            await Arma3ToolsHelper.ImageToPAA(new NoProgress(),name + "-01.png");
        }
    }
}
