using System.Xml.Linq;
using Pmad.Milsymbol.App6d;
using Pmad.Milsymbol.Icons;

namespace App6dGenerator
{
    internal class IconGenerator
    {
        private readonly List<IconEntry> _icons = new List<IconEntry>();
        private readonly SymbolIconGenerator generator = new SymbolIconGenerator();
        private readonly SymbolIconOptions options = new SymbolIconOptions(){ OutlineWidth = 2, StrokeWidth = 6 };

        public List<IconEntry> Icons => _icons;

        public void Process(App6dSymbolSet set, string viewBox, App6dStandardIdentity stdid)
        {
            var stdidLetter = (char)('0' + stdid);

            if (set.Modifiers1.Count > 1 || set.Modifiers2.Count > 1 || set.Amplifiers.Count > 1)
            {
                var builder = new App6dSymbolIdBuilder();
                builder.StandardIdentity = stdid;
                builder.SymbolSet = set.Code;
                var neutralSymbol = generator.Generate(builder.ToSIDC(), options);
                var neutralSvg = XDocument.Parse(neutralSymbol.Svg);

                foreach (var mod1 in set.Modifiers1)
                {
                    if (mod1.Code != "00")
                    {
                        builder.Amplifier = "00";
                        builder.Modifier1 = mod1.Code ?? throw new Exception();
                        builder.Modifier2 = "00";
                        var sdic = builder.ToSIDC();
                        var ms = generator.Generate(sdic, options);
                        var actual = Diff(neutralSvg, XDocument.Parse(ms.Svg), viewBox);
                        AddIcon(stdidLetter, set.Code, $"xxxxxxxxxx{mod1.Code}xx_ca.png", actual);
                    }
                }

                foreach (var mod2 in set.Modifiers2)
                {
                    if (mod2.Code != "00")
                    {
                        builder.Amplifier = "00";
                        builder.Modifier1 = "00";
                        builder.Modifier2 = mod2.Code ?? throw new Exception();
                        var sdic = builder.ToSIDC();
                        var ms = generator.Generate(sdic, options);
                        var actual = Diff(neutralSvg, XDocument.Parse(ms.Svg), viewBox);
                        AddIcon(stdidLetter, set.Code, $"xxxxxxxxxxxx{mod2.Code}_ca.png", actual);
                    }
                }

                if (set.Code == "10")
                {
                    foreach (var hqtffd in Enum.GetValues<App6dHqTfFd>())
                    {
                        var l = (char)('0' + hqtffd);
                        foreach (var amp in set.Amplifiers)
                        {
                            if (amp.Code != "00" || hqtffd != App6dHqTfFd.None)
                            {
                                builder.Amplifier = amp.Code ?? throw new Exception();
                                builder.Modifier1 = "00";
                                builder.Modifier2 = "00";
                                builder.HqTfFd = hqtffd;
                                var sdic = builder.ToSIDC();
                                var ms = generator.Generate(sdic, options);
                                var actual = Diff(neutralSvg, XDocument.Parse(ms.Svg), viewBox);
                                AddIcon(stdidLetter, set.Code, $"x{l}{amp.Code}xxxxxxxxxx_ca.png", actual);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var amp in set.Amplifiers)
                    {
                        if (amp.Code != "00")
                        {
                            builder.Amplifier = amp.Code ?? throw new Exception();
                            builder.Modifier1 = "00";
                            builder.Modifier2 = "00";
                            var sdic = builder.ToSIDC();
                            var ms = generator.Generate(sdic, options);
                            var actual = Diff(neutralSvg, XDocument.Parse(ms.Svg), viewBox);
                            AddIcon(stdidLetter, set.Code, $"x0{amp.Code}xxxxxxxxxx_ca.png", actual);
                        }
                    }
                }

                foreach (var status in Enum.GetValues<App6dStatus>())
                {
                    var l = (char)('0' + status);

                    if (status != App6dStatus.Present)
                    {
                        builder.Amplifier = "00";
                        builder.Modifier1 = "00";
                        builder.Modifier2 = "00";
                        builder.HqTfFd = App6dHqTfFd.None;
                        builder.Status = status;
                        var sdic = builder.ToSIDC();
                        var ms = generator.Generate(sdic, options);
                        var actual = Diff(neutralSvg, XDocument.Parse(ms.Svg), viewBox);
                        AddIcon(stdidLetter, set.Code, $"{l}xxxxxxxxxxxxx_ca.png", actual);
                    }
                }

            }

            foreach (var icon in set.MainIcons)
            {
                if (icon.IsPointRendering)
                {
                    var builder = new App6dSymbolIdBuilder();
                    builder.StandardIdentity = stdid;
                    builder.SymbolSet = set.Code;
                    builder.Icon = icon.Code;
                    var sdic = builder.ToSIDC();
                    var ms = generator.Generate(sdic, options);
                    var actual = NormalizeViewBox(XDocument.Parse(ms.Svg), viewBox);
                    AddIcon(stdidLetter, set.Code, $"xxxx{icon.Code}xxxx_ca.png", actual);
                }
            }
        }

        private void AddIcon(char stdid, string set, string name, XDocument svg)
        {
            if (!svg.Root!.Elements().Any())
            {
                return;
            }
            var str = svg.ToString();
            _icons.Add(new IconEntry(stdid, set, name, str));
        }

        private static XDocument Diff(XDocument neutralSvg, XDocument symbolSvg, string viewBox)
        {
            foreach (var element in neutralSvg.Root!.Elements())
            {
                var matchingElement = symbolSvg.Root!.Elements().FirstOrDefault(e => e.ToString() == element.ToString());
                matchingElement?.Remove();
            }
            NormalizeViewBox(symbolSvg, viewBox);
            return symbolSvg;
        }

        private static XDocument NormalizeViewBox(XDocument symbolSvg, string viewBox)
        {
            symbolSvg.Root!.SetAttributeValue("viewBox", viewBox);
            symbolSvg.Root.SetAttributeValue("width", "256");
            symbolSvg.Root.SetAttributeValue("height", "256");
            return symbolSvg;
        }
    }
}
