using System.Threading.Tasks;
using UnityEngine;

namespace LostKaiju.Services.Saves
{
    public class PlayerPrefsStorage : IAsyncDataStorage
    {
        public Task WriteAsync(string key, string serializedData)
        {
            PlayerPrefs.SetString(key, serializedData);
            return Task.CompletedTask;
        }
        public Task<string> ReadAsync(string key)
        {
            var serializedData = PlayerPrefs.GetString(key);
            return Task.FromResult(serializedData);
        }

        public Task DeleteAsync(string key)
        {
            PlayerPrefs.DeleteKey(key);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            var exists = PlayerPrefs.HasKey(key);
            return Task.FromResult(exists);
        }
    }
}