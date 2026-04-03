using System.Threading;
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
            var data = new { Value = 42 };

            recording.Append("TestEvent", data);

            var events = recording.TakeSnapshot();

            Assert.Single(events);
            Assert.Equal("TestEvent", events[0].Type);
            Assert.Equal(data, events[0].Data);
        }

        [Fact]
        public void Append_UpdatesLastEventAt()
        {
            var recording = new ActiveRecording();
            var before = DateTime.UtcNow;

            recording.Append("TestEvent", new object());

            var after = DateTime.UtcNow;
            Assert.InRange(recording.LastEventAt, before, after);
        }

        [Fact]
        public void Events_ReflectsMultipleAppends()
        {
            var recording = new ActiveRecording();

            recording.Append("Event1", "data1");
            recording.Append("Event2", "data2");
            recording.Append("Event3", "data3");

            var events = recording.TakeSnapshot();

            Assert.Equal(3, events.Count);
            Assert.Equal("Event1", events[0].Type);
            Assert.Equal("Event2", events[1].Type);
            Assert.Equal("Event3", events[2].Type);
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
                        recording.Append($"Event-{threadIndex}", j);
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();

            var events = recording.TakeSnapshot();

            Assert.Equal(threadCount * eventsPerThread, events.Count);
        }
    }
}
