using System.Threading;
using cTabWebApp;
using cTabWebApp.Services.Recording;

namespace cTabWebAppTest.Recording
{
    public class ActiveRecordingTest
    {
        [Fact]
        public void StartedAt_IsSetOnConstruction()
        {
            var before = DateTime.UtcNow;
            var recording = new ActiveRecording();
            var after = DateTime.UtcNow;

            Assert.InRange(recording.StartedAt, before, after);
        }

        [Fact]
        public void LastEventAt_IsSetOnConstruction()
        {
            var before = DateTime.UtcNow;
            var recording = new ActiveRecording();
            var after = DateTime.UtcNow;

            Assert.InRange(recording.LastEventAt, before, after);
        }

        [Fact]
        public void Append_AddsEventWithCorrectTypeAndData()
        {
            var recording = new ActiveRecording();
            var data = new TestMessage("data");

            recording.Append(EventType.Mission, data);

            var events = recording.TakeSnapshot();

            Assert.Single(events);
            Assert.Equal(EventType.Mission, events[0].Type);
            Assert.Equal(data, events[0].Data);
        }

        [Fact]
        public void Append_UpdatesLastEventAt()
        {
            var recording = new ActiveRecording();
            var before = DateTime.UtcNow;

            recording.Append(EventType.Mission, new TestMessage("data"));

            var after = DateTime.UtcNow;
            Assert.InRange(recording.LastEventAt, before, after);
        }

        [Fact]
        public void Events_ReflectsMultipleAppends()
        {
            var recording = new ActiveRecording();

            recording.Append(EventType.Mission, new TestMessage("data1"));
            recording.Append(EventType.UpdateMarkers, new TestMessage("data2"));
            recording.Append(EventType.UpdateMapMarkers, new TestMessage("data3"));

            var events = recording.TakeSnapshot();

            Assert.Equal(3, events.Count);
            Assert.Equal(EventType.Mission, events[0].Type);
            Assert.Equal(EventType.UpdateMarkers, events[1].Type);
            Assert.Equal(EventType.UpdateMapMarkers, events[2].Type);
        }

        [Theory]
        [InlineData(EventType.SetPosition)]
        [InlineData(EventType.UpdateMarkersPosition)]
        public void Append_RateLimitedType_DropsEventWithinWindow(EventType type)
        {
            var recording = new ActiveRecording();

            recording.Append(type, new TestMessage("first"));
            recording.Append(type, new TestMessage("second"));

            var events = recording.TakeSnapshot();
            Assert.Single(events);
            Assert.Equal("first", ((TestMessage)events[0].Data).Content);
        }

        [Theory]
        [InlineData(EventType.SetPosition)]
        [InlineData(EventType.UpdateMarkersPosition)]
        public void Append_RateLimitedType_DoesNotDropUnrelatedTypes(EventType type)
        {
            var recording = new ActiveRecording();

            recording.Append(type, new TestMessage("pos"));
            recording.Append(EventType.Mission, new TestMessage("world"));

            var events = recording.TakeSnapshot();
            Assert.Equal(2, events.Count);
        }

        [Fact]
        public void Append_NonRateLimitedType_AlwaysRecordsAllEvents()
        {
            var recording = new ActiveRecording();

            recording.Append(EventType.Mission, new TestMessage("a"));
            recording.Append(EventType.Mission, new TestMessage("b"));
            recording.Append(EventType.Mission, new TestMessage("c"));

            var events = recording.TakeSnapshot();
            Assert.Equal(3, events.Count);
        }

        [Fact]
        public void Append_KeepLatest_UpdatesDataInPlaceWithinWindow()
        {
            var recording = new ActiveRecording();

            recording.Append(EventType.UpdateMapMarkers, new TestMessage("first"));
            recording.Append(EventType.UpdateMapMarkers, new TestMessage("second"));
            recording.Append(EventType.UpdateMapMarkers, new TestMessage("third"));

            var events = recording.TakeSnapshot();
            Assert.Single(events);
            Assert.Equal("third", ((TestMessage)events[0].Data).Content);
        }

        [Fact]
        public void Append_KeepLatest_PreservesOriginalPositionInList()
        {
            var recording = new ActiveRecording();

            recording.Append(EventType.Mission, new TestMessage("world"));
            recording.Append(EventType.UpdateMapMarkers, new TestMessage("first"));
            recording.Append(EventType.UpdateMapMarkers, new TestMessage("latest"));

            var events = recording.TakeSnapshot();
            Assert.Equal(2, events.Count);
            Assert.Equal(EventType.Mission, events[0].Type);
            Assert.Equal(EventType.UpdateMapMarkers, events[1].Type);
            Assert.Equal("latest", ((TestMessage)events[1].Data).Content);
        }

        [Fact]
        public void Append_KeepLatest_DoesNotAffectUnrelatedTypes()
        {
            var recording = new ActiveRecording();

            recording.Append(EventType.UpdateMapMarkers, new TestMessage("markers"));
            recording.Append(EventType.Mission, new TestMessage("world"));

            var events = recording.TakeSnapshot();
            Assert.Equal(2, events.Count);
            Assert.Equal(EventType.Mission, events[1].Type);
        }

        [Fact]
        public void Append_IsThreadSafe()
        {
            var recording = new ActiveRecording();
            const int threadCount = 10;
            const int eventsPerThread = 100;

            var threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                var threadIndex = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < eventsPerThread; j++)
                    {
                        recording.Append(EventType.Mission, new TestMessage(j.ToString()));
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();

            var events = recording.TakeSnapshot();

            Assert.Equal(threadCount * eventsPerThread, events.Count);
        }

        private class TestMessage : RecordableMessageBase
        {
            public TestMessage(string content)
            {
                Content = content;
                Timestamp = DateTime.UtcNow;
            }

            public string Content { get; set; }
        }
    }
}
