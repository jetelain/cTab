using System.Text.Json;
using cTabWebApp;
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
                Events = [new SessionEvent { Type = EventType.Mission, Data = new MissionMessage() { WorldName = worldName } }]
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
        public async Task GetByUser_ReturnsEmptyList_WhenNoRecordings()
        {
            var service = CreateService();

            var result = await service.GetByUserAsync("012345678901234567");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByUser_ReturnsStoredRecordings()
        {
            var service = CreateService();
            await service.SaveAsync("01234567890123453", CreateRecording());

            var result = await service.GetByUserAsync("01234567890123453");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByUser_FiltersOutExpiredEntries()
        {
            var service = CreateService(retention: TimeSpan.FromDays(-1));
            await service.SaveAsync("01234567890123454", CreateRecording());

            var result = await service.GetByUserAsync("01234567890123454");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByUser_OrdersByStartDescending()
        {
            var service = CreateService();
            await service.SaveAsync("01234567890123455", CreateRecording(start: DateTime.UtcNow.AddHours(-3)));
            await service.SaveAsync("01234567890123455", CreateRecording(start: DateTime.UtcNow.AddMinutes(-30)));

            var result = await service.GetByUserAsync("01234567890123455");

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

            var result = await service.GetByUserAsync("01234567890123458");
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

            var result = await service.GetByUserAsync("01234567890123459");

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
            var result = await service2.GetByUserAsync("012345678901234510");

            Assert.Single(result);
            Assert.Equal("malden", result[0].WorldName);
        }

        // ── Argument validation ───────────────────────────────────────────────

        [Fact]
        public async Task SaveAsync_ThrowsArgumentException_ForEmptySteamId()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => service.SaveAsync("", CreateRecording()));
        }

        [Fact]
        public async Task SaveAsync_ThrowsArgumentException_ForNonNumericSteamId()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => service.SaveAsync("not-a-number", CreateRecording()));
        }

        [Fact]
        public async Task GetByUserAsync_ThrowsArgumentException_ForInvalidSteamId()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetByUserAsync("invalid!"));
        }

        [Fact]
        public void OpenRecording_ThrowsArgumentException_ForInvalidSteamId()
        {
            var service = CreateService();
            var stored = new StoredRecording { Token = "token", SteamId = "not-numeric" };
            Assert.Throws<ArgumentException>(() => service.OpenRecording(stored));
        }

        // ── EnsureUserLoadedLocked – on-load validation ───────────────────────

        [Fact]
        public async Task GetByUserAsync_IgnoresAndDeletesCorruptMetadataFile()
        {
            var userDir = Path.Combine(_tempDir, "76561198000000001");
            Directory.CreateDirectory(userDir);
            await File.WriteAllTextAsync(Path.Combine(userDir, "sometoken.json"), "{invalid json{{");

            var service = CreateService();
            var result = await service.GetByUserAsync("76561198000000001");

            Assert.Empty(result);
            Assert.Empty(Directory.GetFiles(userDir, "*.json"));
        }

        [Fact]
        public async Task GetByUserAsync_DeletesMetadataWhoseTokenDoesNotMatchFilename()
        {
            var userDir = Path.Combine(_tempDir, "76561198000000002");
            Directory.CreateDirectory(userDir);
            var entry = new StoredRecording
            {
                Token = "differenttoken",
                SteamId = "76561198000000002",
                ExpiresUtc = DateTime.UtcNow.AddDays(5)
            };
            await File.WriteAllTextAsync(Path.Combine(userDir, "sometoken.json"),
                JsonSerializer.Serialize(entry, JsonOptions));

            var service = CreateService();
            var result = await service.GetByUserAsync("76561198000000002");

            Assert.Empty(result);
            Assert.Empty(Directory.GetFiles(userDir, "*.json"));
        }

        [Fact]
        public async Task GetByUserAsync_DeletesMetadataWhoseSteamIdDoesNotMatchDirectory()
        {
            var userDir = Path.Combine(_tempDir, "76561198000000003");
            Directory.CreateDirectory(userDir);
            var entry = new StoredRecording
            {
                Token = "sometoken",
                SteamId = "99999999999999999",
                ExpiresUtc = DateTime.UtcNow.AddDays(5)
            };
            await File.WriteAllTextAsync(Path.Combine(userDir, "sometoken.json"),
                JsonSerializer.Serialize(entry, JsonOptions));

            var service = CreateService();
            var result = await service.GetByUserAsync("76561198000000003");

            Assert.Empty(result);
            Assert.Empty(Directory.GetFiles(userDir, "*.json"));
        }

        [Fact]
        public async Task GetByUserAsync_DeletesAlreadyExpiredMetadataOnLoad()
        {
            var userDir = Path.Combine(_tempDir, "76561198000000004");
            Directory.CreateDirectory(userDir);
            var entry = new StoredRecording
            {
                Token = "sometoken",
                SteamId = "76561198000000004",
                ExpiresUtc = DateTime.UtcNow.AddDays(-1)
            };
            await File.WriteAllTextAsync(Path.Combine(userDir, "sometoken.json"),
                JsonSerializer.Serialize(entry, JsonOptions));

            var service = CreateService();
            var result = await service.GetByUserAsync("76561198000000004");

            Assert.Empty(result);
            Assert.Empty(Directory.GetFiles(userDir, "*.json"));
        }

        [Fact]
        public async Task GetByUserAsync_DeletesMetadataWhenDataFileIsMissing()
        {
            var userDir = Path.Combine(_tempDir, "76561198000000005");
            Directory.CreateDirectory(userDir);
            var entry = new StoredRecording
            {
                Token = "sometoken",
                SteamId = "76561198000000005",
                ExpiresUtc = DateTime.UtcNow.AddDays(5)
            };
            // Only write the metadata file, no corresponding .json.gz data file
            await File.WriteAllTextAsync(Path.Combine(userDir, "sometoken.json"),
                JsonSerializer.Serialize(entry, JsonOptions));

            var service = CreateService();
            var result = await service.GetByUserAsync("76561198000000005");

            Assert.Empty(result);
            Assert.Empty(Directory.GetFiles(userDir, "*.json"));
        }

        // ── CleanUpAsync extended ─────────────────────────────────────────────

        [Fact]
        public async Task CleanUpAsync_DoesNotThrow_WhenStorageDirectoryWasDeleted()
        {
            var storageDir = Path.Combine(_tempDir, "storage");
            var service = new RecordingStorageService(new RecordingStorageServiceConfig
            {
                StorageLocation = storageDir,
                RetentionDuration = TimeSpan.FromDays(5),
                MaxSessionRecordingCount = 20
            });
            Directory.Delete(storageDir);

            var ex = await Record.ExceptionAsync(() => service.CleanUpAsync());

            Assert.Null(ex);
        }

        [Fact]
        public async Task CleanUpAsync_RemovesExpiredMetadataJsonFiles()
        {
            var service = CreateService(retention: TimeSpan.FromDays(-1));
            await service.SaveAsync("76561198000000006", CreateRecording());

            await service.CleanUpAsync();

            var userDir = Path.Combine(_tempDir, "76561198000000006");
            Assert.Empty(Directory.GetFiles(userDir, "*.json"));
        }

        [Fact]
        public async Task CleanUpAsync_CleansExpiredRecordingsAcrossMultipleUsers()
        {
            var service = CreateService(retention: TimeSpan.FromDays(-1));
            await service.SaveAsync("76561198000000007", CreateRecording());
            await service.SaveAsync("76561198000000008", CreateRecording());

            await service.CleanUpAsync();

            Assert.Empty(Directory.GetFiles(Path.Combine(_tempDir, "76561198000000007"), "*.json.gz"));
            Assert.Empty(Directory.GetFiles(Path.Combine(_tempDir, "76561198000000008"), "*.json.gz"));
        }

        // ── In-memory cache ───────────────────────────────────────────────────

        [Fact]
        public async Task GetByUserAsync_UsesInMemoryCache_DoesNotReloadFromDisk()
        {
            var service = CreateService();
            await service.SaveAsync("76561198000000009", CreateRecording("everon"));

            // Corrupt the on-disk metadata; the in-memory cache should still hold the entry
            foreach (var f in Directory.GetFiles(Path.Combine(_tempDir, "76561198000000009"), "*.json"))
            {
                await File.WriteAllTextAsync(f, "corrupted");
            }

            var result = await service.GetByUserAsync("76561198000000009");

            Assert.Single(result);
            Assert.Equal("everon", result[0].WorldName);
        }
    }
}
