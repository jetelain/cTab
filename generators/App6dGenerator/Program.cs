using System.Diagnostics;
using ExCSS;
using Pmad.Milsymbol.App6d;
using Pmad.ProgressTracking;
using SkiaSharp;
using Svg.Skia;

namespace App6dGenerator
{
    public record IconEntry(char StdId, string Set, string Name, string Svg);

    internal class Program
    {

        private static List<string> imageToPaa = new List<string>();

        static async Task Main(string[] args)
        {
            string targetPath = "C:\\Users\\Julien\\source\\repos\\jetelain\\cTab\\@cTab\\addons\\app6d\\data";

            using (var writer = File.CreateText(Path.Combine(targetPath, "app6d.sqf")))
            {
                writer.WriteLine("[");
                var first = true;
                foreach (var set in App6dSymbolDatabase.Default.SymbolSets)
                {
                    if (set.MainIcons.Count == 0) continue;

                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.WriteLine(",");
                    }
                    writer.WriteLine($"[\"{set.Code}\",\"{set.Name}\",");
                    WriteList(writer, set.Amplifiers, a => a.Code, a => a.Name);
                    WriteList(writer, set.Modifiers1, a => a.Code, a => a.FirstModifier);
                    WriteList(writer, set.Modifiers2, a => a.Code, a => a.SecondModifier);
                    WriteList(writer, set.MainIcons.Where(a => a.IsPointRendering), a => a.Code, GetIconLabel, true);
                    writer.Write("]");
                }
                writer.WriteLine("]");
            }
            await Generate(targetPath);
            var x = Directory.GetFiles(targetPath, "*.paa", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
            Console.WriteLine($"Total size: {x/1024.0/1024.0:0.0} MB");
        }

        public static string? GetIconLabel(App6dMainIcon icon)
        {
            if (!string.IsNullOrEmpty(icon.EntitySubtype))
            {
                return $"{icon.Entity}>{icon.EntityType}>{icon.EntitySubtype}";
            }
            if (!string.IsNullOrEmpty(icon.EntityType))
            {
                return $"{icon.Entity}>{icon.EntityType}";
            }
            return icon.Entity;
        }

        private static void WriteList<T>(StreamWriter writer, IEnumerable<T> list, Func<T,string?> code, Func<T, string?> label, bool isLast = false)
        {
            var firstAmp = true;
            writer.WriteLine($"  [ // {typeof(T).Name}");
            if (!list.Any())
            {
                writer.Write($"    [\"00\",\"n/a\"]");
            }
            else
            {
                foreach (var amp in list)
                {
                    if (firstAmp)
                    {
                        firstAmp = false;
                    }
                    else
                    {
                        writer.WriteLine($",");
                    }
                    writer.Write($"    [\"{code(amp)}\",\"{label(amp)}\"]");
                }
            }
            writer.WriteLine(isLast ? "]" : "],");
        }

        private static async Task<ConsoleProgessRender> Generate(string targetPath)
        {
            using var consoleProgess = new ConsoleProgessRender();

            var generator = new IconGenerator();

            foreach (var set in App6dSymbolDatabase.Default.SymbolSets.WithProgress(consoleProgess, "SymbolSets"))
            {
                var viewBox = "-50 -50 300 300"; // Standard viewBox for all symbols
                if (set.Code == "25")
                {
                    viewBox = "-100 -100 400 400"; // Special viewBox for symbol set 25 - Control measure
                }
                foreach (var stdid in Enum.GetValues<App6dStandardIdentity>())
                {
                    consoleProgess.WriteLine($"Processing {set.Code} {stdid}");
                    generator.Process(set, viewBox, stdid);
                }
            }

            foreach (var grp in generator.Icons.GroupBy(n => new { n.Name, n.Set }).WithProgress(consoleProgess, "Icons"))
            {
                consoleProgess.WriteLine($"Processing {grp.Key.Set} {grp.Key.Name}");

                var svgGrps = grp.GroupBy(n => n.Svg).OrderByDescending(g => g.Count()).ToList();
                var shared = svgGrps[0];
                if (shared.Count() > 1) // 
                {
                    StoreShared(targetPath, shared.First());

                    foreach (var other in svgGrps.Skip(1))
                    {
                        foreach (var item in other)
                        {
                            Store(targetPath, item);
                        }
                    }
                }
                else
                {
                    foreach (var other in svgGrps)
                    {
                        foreach (var item in other)
                        {
                            Store(targetPath, item);
                        }
                    }
                }
            }

            Directory.CreateDirectory(Path.Combine(targetPath, "o"));   
            ReinforcedReduced(Path.Combine(targetPath, "o\\plus_ca.png"), generator, "(+)");
            ReinforcedReduced(Path.Combine(targetPath, "o\\minus_ca.png"), generator, "(-)");
            ReinforcedReduced(Path.Combine(targetPath, "o\\plusminus_ca.png"), generator, "(±)");

            await Arma3ToolsHelper.ImageToPAA(consoleProgess, imageToPaa);
            return consoleProgess;
        }

        private static void Store(string target, IconEntry item)
        {
            var dir = Path.Combine(target, item.StdId.ToString(), item.Set);
            Directory.CreateDirectory(dir);
            File.WriteAllBytes(Path.Combine(dir, item.Name), ToPng(item.Svg));
            imageToPaa.Add(Path.Combine(dir, item.Name));
        }

        private static void StoreShared(string target, IconEntry item)
        {
            var dir = Path.Combine(target, "s", item.Set);
            Directory.CreateDirectory(dir);
            File.WriteAllBytes(Path.Combine(dir, item.Name), ToPng(item.Svg));
            imageToPaa.Add(Path.Combine(dir, item.Name));
        }

        private static void ReinforcedReduced(string target, IconGenerator generator, string text)
        {
            File.WriteAllBytes(target, ToPng(generator.ReinforcedReduced(text).ToString()));
            imageToPaa.Add(target);
        }

        public static void SaveToPng(string svg, Stream target, float scale = 1f)
        {
            using (var xsvg = new SKSvg())
            {
                if (xsvg.FromSvg(svg) == null)
                {
                    throw new InvalidOperationException("Generated SVG seems invalid");
                }
                xsvg.Save(target, SKColor.Empty, SKEncodedImageFormat.Png, 100, scale, scale);
            }
        }
        public static byte[] ToPng(string icon, float scale = 1f)
        {
            var mem = new MemoryStream();
            SaveToPng(icon, mem, scale);
            return mem.ToArray();
        }
    }
}