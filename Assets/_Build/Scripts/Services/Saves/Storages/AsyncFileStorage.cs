using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LostKaiju.Services.Saves
{
    public class AsyncFileStorage : IAsyncDataStorage
    {
        private readonly string _basePath;
        private readonly string _fileExtension;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _ctsMap;

        public AsyncFileStorage(string fileExtension)
        {
            _basePath = Application.isEditor ? Path.Combine(Application.dataPath, "SaveData") : Application.persistentDataPath;
            _fileExtension = fileExtension;
            _ctsMap = new ConcurrentDictionary<string, CancellationTokenSource>();
        }

        public async Task WriteAsync(string key, string serializedData)
        {
            var path = GetPath(key);
            var cts = _ctsMap.AddOrUpdate(key,
                _ => new CancellationTokenSource(),
                (_, oldCts) =>
                {
                    oldCts.Cancel();
                    oldCts.Dispose();

                    return new CancellationTokenSource();
                });

            try
            {
                await File.WriteAllTextAsync(path, serializedData, cts.Token);

                _ctsMap.TryRemove(key, out _);
                cts.Dispose();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("FileStorage: Save operation was canceled.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"FileStorage: Save operation failed: {ex.Message}");
            }
        }

        public Task<string> ReadAsync(string key)
        {
            var path = GetPath(key);
            var task = File.ReadAllTextAsync(path);

            return task;
        }

        public Task DeleteAsync(string key)
        {
            var path = GetPath(key);

            if (File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            var path = GetPath(key);
            var exists = File.Exists(path);

            return Task.FromResult(exists);
        }

        private string GetPath(string key)
        {
            var path = Path.Combine(_basePath, String.Concat(key, ".", _fileExtension));
            return path;
        }
    }
}