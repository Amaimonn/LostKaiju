using System.Threading.Tasks;

namespace LostKaiju.Services.Saves
{
    public class SimpleSaveSystem : IAsyncSaveSystem
    {
        private readonly IAsyncSerializer _serializer;
        private readonly IAsyncDataStorage _storage;

        public SimpleSaveSystem(IAsyncSerializer serializer, IAsyncDataStorage storage)
        {
            _serializer = serializer;
            _storage = storage;
        }

        public Task SaveAsync<T>(string key, T data)
        {
            var serializedData = _serializer.SerializeAsync(data).Result;
            _storage.WriteAsync(key, serializedData);

            return Task.CompletedTask;
        }

        public Task<T> LoadAsync<T>(string key)
        {
            var serializedData = _storage.ReadAsync(key).Result;
            var data = _serializer.DeserializeAsync<T>(serializedData);

            return data;
        }

        public Task DeleteAsync(string key)
        {
            _storage.DeleteAsync(key);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            var exists = _storage.ExistsAsync(key);
            return exists;
        }
    }
}