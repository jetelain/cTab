using System.Text.Json;
using cTabWebApp.Services.Recording;

namespace cTabWebAppTest.Recording
{
    public class RecordingStorageServiceTest : IDisposable
    {
        private readonly string _tempDir;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RecordingStorageServiceTest()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "cTabWebAppTest_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, recursive: true);
            }
        }

        private RecordingStorageService CreateService(TimeSpan? retention = null, int maxPerUser = 20)
            => new RecordingStorageService(new RecordingStorageServiceConfig
            {
                StorageLocation = _tempDir,
                RetentionDuration = retention ?? TimeSpan.FromDays(5),
                MaxSessionRecordingCount = maxPerUser
            });

        private static SessionRecording CreateRecording(string worldName = "altis", DateTime? start = null)
        {
            var now = DateTime.UtcNow;
            return new SessionRecording
            {
                WorldName = worldName,
                RecordingStart = start ?? now.AddMinutes(-10),
                RecordingEnd = now,
                Events = [new SessionEvent { Type = "Mission", Data = worldName }]
            };
        }

        // ── SaveAsync ─────────────────────────────────────────────────────────

        [Fact]
        public async Task SaveAsync_ReturnsStoredRecordingWithCorrectFields()
        {
            var service = CreateService();
            var recording = CreateRecording("altis");

            var stored = await service.SaveAsync("01234567890123451", recording);

            Assert.NotNull(stored);
            Assert.Equal("01234567890123451", stored!.SteamId);
            Assert.Equal("altis", stored.WorldName);
            Assert.Equal(recording.RecordingStart, stored.RecordingStart);
            Assert.Equal(recording.RecordingEnd, stored.RecordingEnd);
            Assert.NotEmpty(stored.Token);
        }

        [Fact]
        public async Task SaveAsync_WritesFilesToDisk()
        {
            var service = CreateService();

            var stored = await service.SaveAsync("01234567890123452", CreateRecording());

            var userDir = Path.Combine(_tempDir, "01234567890123452");
            Assert.True(File.Exists(Path.Combine(userDir, stored!.Token + ".json")));
            Assert.True(File.Exists(Path.Combine(userDir, stored.Token + ".json.gz")));
        }

        // ── GetByUser ─────────────────────────────────────────────────────────

        [Fact]
        public void GetByUser_ReturnsEmptyList_WhenNoRecordings()
        {
            var service = CreateService();

            var result = service.GetByUser("012345678901234567");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByUser_ReturnsStoredRecordings()
        {
            var service = CreateService();
            await service.SaveAsync("01234567890123453", CreateRecording());

            var result = service.GetByUser("01234567890123453");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByUser_FiltersOutExpiredEntries()
        {
            var service = CreateService(retention: TimeSpan.FromDays(-1));
            await service.SaveAsync("01234567890123454", CreateRecording());

            var result = service.GetByUser("01234567890123454");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByUser_OrdersByStartDescending()
        {
            var service = CreateService();
            await service.SaveAsync("01234567890123455", CreateRecording(start: DateTime.UtcNow.AddHours(-3)));
            await service.SaveAsync("01234567890123455", CreateRecording(start: DateTime.UtcNow.AddMinutes(-30)));

            var result = service.GetByUser("01234567890123455");

            Assert.Equal(2, result.Count);
            Assert.True(result[0].RecordingStart > result[1].RecordingStart);
        }

        // ── OpenRecording ─────────────────────────────────────────────────────

        [Fact]
        public async Task OpenRecording_ReturnsReadableStreamWithCorrectContent()
        {
            var service = CreateService();
            var stored = await service.SaveAsync("01234567890123456", CreateRecording("stratis"));

            var stream = service.OpenRecording(stored!);
            Assert.NotNull(stream);
            await using (stream)
            {
                var deserialized = await JsonSerializer.DeserializeAsync<SessionRecording>(stream!, JsonOptions);
                Assert.NotNull(deserialized);
                Assert.Equal("stratis", deserialized!.WorldName);
            }
        }

        [Fact]
        public void OpenRecording_ReturnsNull_WhenFileDoesNotExist()
        {
            var service = CreateService();
            var stored = new StoredRecording { Token = "nonexistent", SteamId = "01234567890123450" };

            var stream = service.OpenRecording(stored);

            Assert.Null(stream);
        }

        // ── CleanUpAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CleanUpAsync_RemovesExpiredFiles()
        {
            var service = CreateService(retention: TimeSpan.FromDays(-1));
            await service.SaveAsync("01234567890123457", CreateRecording());

            await service.CleanUpAsync();

            var userDir = Path.Combine(_tempDir, "01234567890123457");
            Assert.Empty(Directory.GetFiles(userDir, "*.json.gz"));
        }

        [Fact]
        public async Task CleanUpAsync_DoesNotRemoveNonExpiredEntries()
        {
            var service = CreateService();
            await service.SaveAsync("01234567890123458", CreateRecording());

            await service.CleanUpAsync();

            var result = service.GetByUser("01234567890123458");
            Assert.Single(result);
        }

        // ── Quota enforcement ─────────────────────────────────────────────────

        [Fact]
        public async Task SaveAsync_EnforcesUserQuota_RemovesOldest()
        {
            var service = CreateService(maxPerUser: 2);
            var s1 = await service.SaveAsync("01234567890123459", CreateRecording(start: DateTime.UtcNow.AddHours(-3)));
            await service.SaveAsync("01234567890123459", CreateRecording(start: DateTime.UtcNow.AddHours(-2)));
            await service.SaveAsync("01234567890123459", CreateRecording(start: DateTime.UtcNow.AddHours(-1)));

            var result = service.GetByUser("01234567890123459");

            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, r => r.Token == s1!.Token);
        }

        // ── Persistence ───────────────────────────────────────────────────────

        [Fact]
        public async Task GetByUser_LoadsPersistedDataFromDisk_AfterNewServiceInstance()
        {
            var service1 = CreateService();
            await service1.SaveAsync("012345678901234510", CreateRecording("malden"));

            var service2 = CreateService();
            var result = service2.GetByUser("012345678901234510");

            Assert.Single(result);
            Assert.Equal("malden", result[0].WorldName);
        }
    }
}
