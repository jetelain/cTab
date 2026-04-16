using System.Collections.Generic;
using System.Text.Json;
using cTabWebApp;

namespace cTabWebAppTest.Hub
{
    /// <summary>
    /// Tests for <see cref="UpdateMapMarkersMessage"/> covering value equality
    /// (<see cref="IEquatable{T}"/>) and the JSON serialisation contract consumed
    /// by the JS client and the replay engine.
    /// Production serialisers use PropertyNamingPolicy = CamelCase.
    /// </summary>
    public class UpdateMapMarkersMessageTest
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        // ── Test-data factories ───────────────────────────────────────────────

        private static SimpleMapMarker MakeSimple() => new SimpleMapMarker
        {
            Name  = "marker1",
            Alpha = 1.0,
            Pos   = [100.0, 200.0],
            Dir   = 45.0,
            Size  = [2.0, 2.0],
            Shape = "rectangle",
            Brush = "solid",
            Color = "#FF0000"
        };

        private static PolylineMapMarker MakePolyline() => new PolylineMapMarker
        {
            Name   = "poly1",
            Alpha  = 0.8,
            Points = [10.0, 20.0, 30.0, 40.0],
            Brush  = "dashed",
            Color  = "#00FF00"
        };

        private static IconMapMarker MakeIcon() => new IconMapMarker
        {
            Name  = "icon1",
            Alpha = 0.5,
            Pos   = [50.0, 60.0],
            Icon  = "ColorBlue/hd_kill.png",
            Size  = [1.5, 1.5],
            Dir   = 90.0,
            Label = "Alpha"
        };

        private static UpdateMapMarkersMessage CreateMessage() => new UpdateMapMarkersMessage
        {
            Simples   = [MakeSimple()],
            Polylines = [MakePolyline()],
            Icons     = [MakeIcon()]
        };

        // ── Equality ─────────────────────────────────────────────────────────

        [Fact]
        public void Equals_SameInstance_ReturnsTrue()
        {
            var msg = CreateMessage();

            Assert.True(msg.Equals(msg));
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            Assert.False(CreateMessage().Equals((UpdateMapMarkersMessage?)null));
        }

        [Fact]
        public void Equals_BothEmptyLists_ReturnsTrue()
        {
            var a = new UpdateMapMarkersMessage { Simples = [], Polylines = [], Icons = [] };
            var b = new UpdateMapMarkersMessage { Simples = [], Polylines = [], Icons = [] };

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_IdenticalContent_ReturnsTrue()
        {
            Assert.True(CreateMessage().Equals(CreateMessage()));
        }

        [Fact]
        public void Equals_DifferentSimpleName_ReturnsFalse()
        {
            var a = CreateMessage();
            var b = CreateMessage();
            b.Simples[0] = new SimpleMapMarker
            {
                Name  = "other",
                Alpha = 1.0,
                Pos   = [100.0, 200.0],
                Dir   = 45.0,
                Size  = [2.0, 2.0],
                Shape = "rectangle",
                Brush = "solid",
                Color = "#FF0000"
            };

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentSimplePosArray_ReturnsFalse()
        {
            var a = CreateMessage();
            var b = CreateMessage();
            b.Simples[0] = new SimpleMapMarker
            {
                Name  = "marker1",
                Alpha = 1.0,
                Pos   = [999.0, 200.0],  // one coordinate changed
                Dir   = 45.0,
                Size  = [2.0, 2.0],
                Shape = "rectangle",
                Brush = "solid",
                Color = "#FF0000"
            };

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentPolylinePoints_ReturnsFalse()
        {
            var a = CreateMessage();
            var b = CreateMessage();
            b.Polylines[0] = new PolylineMapMarker
            {
                Name   = "poly1",
                Alpha  = 0.8,
                Points = [10.0, 20.0, 30.0, 99.0],  // last point changed
                Brush  = "dashed",
                Color  = "#00FF00"
            };

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentIconPosArray_ReturnsFalse()
        {
            var a = CreateMessage();
            var b = CreateMessage();
            b.Icons[0] = new IconMapMarker
            {
                Name  = "icon1",
                Alpha = 0.5,
                Pos   = [50.0, 99.0],  // Y changed
                Icon  = "ColorBlue/hd_kill.png",
                Size  = [1.5, 1.5],
                Dir   = 90.0,
                Label = "Alpha"
            };

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentIconName_ReturnsFalse()
        {
            var a = CreateMessage();
            var b = CreateMessage();
            b.Icons[0] = new IconMapMarker
            {
                Name  = "icon1",
                Alpha = 0.5,
                Pos   = [50.0, 60.0],
                Icon  = "other.png",
                Size  = [1.5, 1.5],
                Dir   = 90.0,
                Label = "Alpha"
            };

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_ExtraSimpleInList_ReturnsFalse()
        {
            var a = CreateMessage();
            var b = CreateMessage();
            b.Simples.Add(MakeSimple());

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_ObjectOverload_Null_ReturnsFalse()
        {
            Assert.False(CreateMessage().Equals((object?)null));
        }

        [Fact]
        public void Equals_ObjectOverload_WrongType_ReturnsFalse()
        {
            Assert.False(CreateMessage().Equals("not a message"));
        }

        [Fact]
        public void Equals_ObjectOverload_EqualMessage_ReturnsTrue()
        {
            var a = CreateMessage();
            var b = CreateMessage();

            Assert.True(a.Equals((object)b));
        }

        [Fact]
        public void GetHashCode_EqualMessages_ReturnSameValue()
        {
            var a = CreateMessage();
            var b = CreateMessage();

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        // ── JSON serialisation ────────────────────────────────────────────────

        [Fact]
        public void Serialize_EmptyLists_ProducesExactJson()
        {
            var msg = new UpdateMapMarkersMessage { Simples = [], Polylines = [], Icons = [] };

            var json = JsonSerializer.Serialize(msg, Options);

            Assert.Equal("{\"simples\":[],\"polylines\":[],\"icons\":[]}", json);
        }

        [Fact]
        public void Serialize_TimestampIsIgnored()
        {
            var msg = new UpdateMapMarkersMessage { Simples = [], Polylines = [], Icons = [] };

            var json = JsonSerializer.Serialize(msg, Options);

            Assert.DoesNotContain("\"timestamp\"", json);
            Assert.DoesNotContain("\"Timestamp\"", json);
        }

        [Fact]
        public void Serialize_SimpleMapMarker_PropertyNamesAreCamelCase()
        {
            var msg = new UpdateMapMarkersMessage
            {
                Simples   = [MakeSimple()],
                Polylines = [],
                Icons     = []
            };

            var json = JsonSerializer.Serialize(msg, Options);

            Assert.Contains("\"name\":\"marker1\"",   json);
            Assert.Contains("\"alpha\":1",            json);
            Assert.Contains("\"pos\":[100,200]",      json);
            Assert.Contains("\"dir\":45",             json);
            Assert.Contains("\"size\":[2,2]",         json);
            Assert.Contains("\"shape\":\"rectangle\"",json);
            Assert.Contains("\"brush\":\"solid\"",    json);
            Assert.Contains("\"color\":\"#FF0000\"",  json);
            Assert.DoesNotContain("\"Name\"",         json);
            Assert.DoesNotContain("\"Dir\"",          json);
            Assert.DoesNotContain("\"Shape\"",        json);
        }

        [Fact]
        public void Serialize_PolylineMapMarker_PropertyNamesAreCamelCase()
        {
            var msg = new UpdateMapMarkersMessage
            {
                Simples   = [],
                Polylines = [MakePolyline()],
                Icons     = []
            };

            var json = JsonSerializer.Serialize(msg, Options);

            Assert.Contains("\"name\":\"poly1\"",          json);
            Assert.Contains("\"alpha\":0.8",               json);
            Assert.Contains("\"points\":[10,20,30,40]",    json);
            Assert.Contains("\"brush\":\"dashed\"",        json);
            Assert.Contains("\"color\":\"#00FF00\"",       json);
            Assert.DoesNotContain("\"Points\"",            json);
            Assert.DoesNotContain("\"Brush\"",             json);
        }

        [Fact]
        public void Serialize_IconMapMarker_PropertyNamesAreCamelCase()
        {
            var msg = new UpdateMapMarkersMessage
            {
                Simples   = [],
                Polylines = [],
                Icons     = [MakeIcon()]
            };

            var json = JsonSerializer.Serialize(msg, Options);

            Assert.Contains("\"name\":\"icon1\"",                    json);
            Assert.Contains("\"alpha\":0.5",                         json);
            Assert.Contains("\"pos\":[50,60]",                       json);
            Assert.Contains("\"icon\":\"ColorBlue/hd_kill.png\"",    json);
            Assert.Contains("\"size\":[1.5,1.5]",                    json);
            Assert.Contains("\"dir\":90",                            json);
            Assert.Contains("\"label\":\"Alpha\"",                   json);
            Assert.DoesNotContain("\"Icon\"",                        json);
            Assert.DoesNotContain("\"Label\"",                       json);
            Assert.DoesNotContain("\"Pos\"",                         json);
        }

        [Fact]
        public void Serialize_MultipleMarkersOfEachType_AllItemsAreIncluded()
        {
            var msg = new UpdateMapMarkersMessage
            {
                Simples   = [MakeSimple(), new SimpleMapMarker { Name = "marker2", Alpha = 0.5, Pos = [1.0, 2.0], Size = [1.0, 1.0], Shape = "ellipse", Dir = 0 }],
                Polylines = [MakePolyline(), new PolylineMapMarker { Name = "poly2", Alpha = 1.0, Points = [1.0, 2.0], Brush = "solid", Color = "#0000FF" }],
                Icons     = [MakeIcon(), new IconMapMarker { Name = "icon2", Alpha = 1.0, Pos = [1.0, 2.0], Icon = "other.png", Size = [1.0, 1.0], Dir = 0, Label = "B" }]
            };

            var json = JsonSerializer.Serialize(msg, Options);

            Assert.Contains("\"marker1\"", json);
            Assert.Contains("\"marker2\"", json);
            Assert.Contains("\"poly1\"",   json);
            Assert.Contains("\"poly2\"",   json);
            Assert.Contains("\"icon1\"",   json);
            Assert.Contains("\"icon2\"",   json);
        }
    }
}
