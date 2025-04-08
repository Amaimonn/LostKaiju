using System;
using System.IO;
using UnityEngine;

namespace LostKaiju.Services.Saves
{
    public class FileStorage : IDataStorage
    {
        private readonly string _basePath;
        private readonly string _fileExtension;

        public FileStorage(string fileExtension)
        {
            _basePath = Application.isEditor ? Path.Combine(Application.dataPath, "SaveData") : Application.persistentDataPath;
            _fileExtension = fileExtension;
        }

        public void Write(string key, string serializedData)
        {
            var path = GetPath(key);

            try
            {
                File.WriteAllText(path, serializedData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"FileStorage: Save operation failed: {ex.Message}");
            }
        }

        public string Read(string key)
        {
            var path = GetPath(key);
            var text = File.ReadAllText(path);

            return text;
        }

        public void Delete(string key)
        {
            var path = GetPath(key);

            if (File.Exists(path))
                File.Delete(path);
        }

        public bool Exists(string key)
        {
            var path = GetPath(key);
            var exists = File.Exists(path);

            return exists;
        }

        private string GetPath(string key)
        {
            var path = Path.Combine(_basePath, String.Concat(key, ".", _fileExtension));
            return path;
        }
    }
}