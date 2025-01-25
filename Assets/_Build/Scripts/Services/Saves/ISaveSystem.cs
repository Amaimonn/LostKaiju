using System.Threading.Tasks;

namespace LostKaiju.Services.Saves
{
    public interface ISaveSystem
    {
        public Task SaveAsync<T>(string key, T data);
        public Task<T> LoadAsync<T>(string key);
        public Task DeleteAsync(string key);
        public Task<bool> ExistsAsync(string key);
    }
}