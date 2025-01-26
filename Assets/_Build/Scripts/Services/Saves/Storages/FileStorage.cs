using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace LostKaiju.Services.Saves
{
    public class FileStorage : IDataStorage
    {
        private string _basePath;
        private string _fileExtension;

        public FileStorage(string fileExtension)
        {
            _basePath = Application.persistentDataPath;
            _fileExtension = fileExtension;
        }

        public Task WriteAsync(string key, string serializedData)
        {
            var path = GetPath(key);
            var task = File.WriteAllTextAsync(path, serializedData);

            return task;
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