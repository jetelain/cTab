using System;
using System.Collections.Generic;
using System.Text.Json;
using cTabWebApp;
using cTabWebApp.Services.Recording;

namespace cTabWebAppTest.Recording
{
    /// <summary>
    /// Verifies the JSON contract of <see cref="SessionEvent"/> as consumed by the JS replay engine.
    /// Production serialisers (RecordingStorageService + SessionController) both use
    /// PropertyNamingPolicy = JsonNamingPolicy.CamelCase.
    /// The EventType enum carries [JsonConverter(typeof(JsonStringEnumConverter))], which must
    /// produce the exact C# member name ("Mission", "SetPosition", …) and must NOT be folded to
    /// camelCase by the naming policy.
    /// </summary>
    public class SessionEventSerializationTest
    {
        // Mirror of the options used in both RecordingStorageService and SessionController.
        private static readonly JsonSerializerOptions ProductionOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        /// <summary>Serialise and return the raw JSON string for inspection.</summary>
        private static string Serialize(EventType type, RecordableMessageBase data)
        {
            var evt = new SessionEvent { Type = type, Data = data, TimestampMs = 1 };
            return JsonSerializer.Serialize(evt, ProductionOptions);
        }

        // ── EventType serialisation contract ──────────────────────────────────

        [Theory]
        [InlineData(EventType.Mission,               "Mission")]
        [InlineData(EventType.SetPosition,           "SetPosition")]
        [InlineData(EventType.UpdateMarkers,         "UpdateMarkers")]
        [InlineData(EventType.UpdateMarkersPosition, "UpdateMarkersPosition")]
        [InlineData(EventType.UpdateMapMarkers,      "UpdateMapMarkers")]
        public void Serialize_Type_IsPascalCaseMemberName_NotIntegerNotCamelCase(EventType eventType, string expectedName)
        {
            var json = Serialize(eventType, new RecordableMessageBase());

            Assert.Contains($"\"type\":\"{expectedName}\"", json);    // exact member name, as a string
            Assert.DoesNotContain($"\"type\":{(int)eventType}", json); // not an integer

            // Naming policy must not fold enum values: "SetPosition" must not become "setPosition"
            var camelCased = char.ToLowerInvariant(expectedName[0]) + expectedName[1..];
            if (expectedName != camelCased)
            {
                Assert.DoesNotContain($"\"type\":\"{camelCased}\"", json);
            }
        }

        // ── Top-level property-name contract ──────────────────────────────────

        [Fact]
        public void Serialize_TopLevelProperties_AreCamelCase()
        {
            var json = Serialize(EventType.Mission, new RecordableMessageBase());

            Assert.Equal("{\"type\":\"Mission\",\"data\":{},\"time\":1}", json);
        }

        // ── Mission ────────────────────────────────────────────────────────────

        [Fact]
        public void Serialize_Mission_DataPropertiesAreCamelCase()
        {
            var msg = new MissionMessage
            {
                WorldName = "altis",
                Size = 20480,
                Date = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc),
                Timestamp = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc),
                CtabFeatureLevel = 1,
                IrlFeatureLevel = 0
            };

            var json = Serialize(EventType.Mission, msg);

            Assert.Equal("{\"type\":\"Mission\",\"data\":{\"worldName\":\"altis\",\"size\":20480,\"date\":\"2024-06-01T08:30:00Z\",\"ctabFeatureLevel\":1,\"irlFeatureLevel\":0},\"time\":1}", json);
        }

        // ── SetPosition ────────────────────────────────────────────────────────

        [Fact]
        public void Serialize_SetPosition_DataPropertiesAreCamelCase()
        {
            var msg = new SetPositionMessage
            {
                X = 12345.6, Y = 9876.5, Altitude = 100, Heading = 270,
                Group = "bravo",
                Date = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc),
                Timestamp = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc)
            };

            var json = Serialize(EventType.SetPosition, msg);

            Assert.Contains("\"type\":\"SetPosition\"", json);
            Assert.Contains("\"x\":12345.6",            json);
            Assert.Contains("\"y\":9876.5",             json);
            Assert.Contains("\"altitude\":100",         json);
            Assert.Contains("\"heading\":270",          json);
            Assert.Contains("\"group\":\"bravo\"",      json);
            Assert.DoesNotContain("\"X\":",             json);
            Assert.DoesNotContain("\"Heading\":",       json);
        }

        [Fact]
        public void Serialize_SetPosition_NullVehicleArraysAreOmitted()
        {
            var json = Serialize(EventType.SetPosition, new SetPositionMessage { X = 1, Y = 2, Heading = 0 });

            Assert.DoesNotContain("\"vhlDir\"", json);
            Assert.DoesNotContain("\"vhlVel\"", json);
            Assert.DoesNotContain("\"vhlPos\"", json);
            Assert.DoesNotContain("\"vhlUp\"",  json);
            Assert.DoesNotContain("\"wind\"",   json);
        }

        [Fact]
        public void Serialize_SetPosition_PresentVehicleArraysAreIncluded()
        {
            var msg = new SetPositionMessage
            {
                X = 1, Y = 2, Heading = 0,
                VhlDir = [0.0, 1.0, 0.0],
                VhlUp  = [0.0, 0.0, 1.0],
                Wind   = [1.0, 0.0, 0.0]
            };

            var json = Serialize(EventType.SetPosition, msg);

            Assert.Contains("\"vhlDir\"",           json);
            Assert.Contains("\"vhlUp\"",            json);
            Assert.Contains("\"wind\"",             json);
            Assert.DoesNotContain("\"vhlVel\"",     json); // still null → omitted
            Assert.DoesNotContain("\"vhlPos\"",     json); // still null → omitted
        }

        // ── UpdateMarkers ──────────────────────────────────────────────────────

        [Fact]
        public void Serialize_UpdateMarkers_DataPropertiesAreCamelCase()
        {
            var msg = new UpdateMarkersMessage
            {
                Timestamp = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc),
                Makers =
                [
                    new Marker { Id = "u1", Kind = "u", X = 100, Y = 200, Heading = 45, Symbol = "abc", Name = "Alpha" }
                ]
            };

            var json = Serialize(EventType.UpdateMarkers, msg);

            Assert.Contains("\"type\":\"UpdateMarkers\"", json);
            Assert.Contains("\"makers\":",               json);
            Assert.Contains("\"id\":\"u1\"",             json);
            Assert.Contains("\"kind\":\"u\"",            json);
            Assert.Contains("\"x\":100",                 json);
            Assert.Contains("\"y\":200",                 json);
            Assert.Contains("\"heading\":45",            json);
            Assert.Contains("\"symbol\":\"abc\"",        json);
            Assert.Contains("\"name\":\"Alpha\"",        json);
            Assert.DoesNotContain("\"Id\"",              json);
        }

        [Fact]
        public void Serialize_UpdateMarkers_NullVehicleAndGroupAreOmitted()
        {
            var json = Serialize(EventType.UpdateMarkers, new UpdateMarkersMessage
            {
                Timestamp = DateTime.UtcNow,
                Makers = [new Marker { Id = "u1", Kind = "g" }]
            });

            Assert.DoesNotContain("\"vehicle\"", json);
            Assert.DoesNotContain("\"group\"",   json);
        }

        [Fact]
        public void Serialize_UpdateMarkers_VehicleAndGroupPresentWhenSet()
        {
            var json = Serialize(EventType.UpdateMarkers, new UpdateMarkersMessage
            {
                Timestamp = DateTime.UtcNow,
                Makers = [new Marker { Id = "u1", Kind = "u", Vehicle = "tank1", Group = "bravo" }]
            });

            Assert.Contains("\"vehicle\":\"tank1\"", json);
            Assert.Contains("\"group\":\"bravo\"",   json);
        }

        // ── UpdateMarkersPosition ──────────────────────────────────────────────

        [Fact]
        public void Serialize_UpdateMarkersPosition_DataPropertiesAreCamelCase()
        {
            var msg = new UpdateMarkersMessagePosition
            {
                Timestamp = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc),
                Makers =
                [
                    new MarkerPosition { Id = "u1", X = 10.5, Y = 20.5, Heading = 90.0 }
                ]
            };

            var json = Serialize(EventType.UpdateMarkersPosition, msg);

            Assert.Contains("\"type\":\"UpdateMarkersPosition\"", json);
            Assert.Contains("\"makers\":",                        json);
            Assert.Contains("\"id\":\"u1\"",                      json);
            Assert.Contains("\"x\":10.5",                         json);
            Assert.Contains("\"y\":20.5",                         json);
            Assert.Contains("\"heading\":90",                     json);
            Assert.DoesNotContain("\"Id\"",                       json);
        }

        // ── UpdateMapMarkers ───────────────────────────────────────────────────

        [Fact]
        public void Serialize_UpdateMapMarkers_DataPropertiesAreCamelCase()
        {
            var msg = new UpdateMapMarkersMessage
            {
                Timestamp = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc),
                Icons =
                [
                    new IconMapMarker { Name = "HQ", Icon = "flag.png", Pos = [100.0, 200.0], Alpha = 1.0, Label = "Base" }
                ],
                Simples =
                [
                    new SimpleMapMarker { Name = "Zone", Shape = "rectangle", Pos = [300.0, 400.0], Color = "ff0000", Alpha = 0.5 }
                ],
                Polylines =
                [
                    new PolylineMapMarker { Name = "Route", Points = [1.0, 2.0, 3.0, 4.0], Color = "00ff00", Alpha = 1.0 }
                ]
            };

            var json = Serialize(EventType.UpdateMapMarkers, msg);

            Assert.Contains("\"type\":\"UpdateMapMarkers\"", json);
            Assert.Contains("\"icons\":",                    json);
            Assert.Contains("\"simples\":",                  json);
            Assert.Contains("\"polylines\":",                json);
            Assert.DoesNotContain("\"Icons\"",               json);

            Assert.Contains("\"name\":\"HQ\"",       json);
            Assert.Contains("\"icon\":\"flag.png\"", json);
            Assert.Contains("\"label\":\"Base\"",    json);
            Assert.Contains("\"pos\":",              json);

            Assert.Contains("\"name\":\"Zone\"",      json);
            Assert.Contains("\"shape\":\"rectangle\"", json);
            Assert.Contains("\"color\":\"ff0000\"",   json);

            Assert.Contains("\"name\":\"Route\"",  json);
            Assert.Contains("\"color\":\"00ff00\"", json);
            Assert.Contains("\"points\":",         json);
        }
    }
}
