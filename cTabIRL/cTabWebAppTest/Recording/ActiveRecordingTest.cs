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

            Assert.Single(recording.Events);
            Assert.Equal("TestEvent", recording.Events[0].Type);
            Assert.Equal(data, recording.Events[0].Data);
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

            Assert.Equal(3, recording.Events.Count);
            Assert.Equal("Event1", recording.Events[0].Type);
            Assert.Equal("Event2", recording.Events[1].Type);
            Assert.Equal("Event3", recording.Events[2].Type);
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

            Assert.Equal(threadCount * eventsPerThread, recording.Events.Count);
        }
    }
}
