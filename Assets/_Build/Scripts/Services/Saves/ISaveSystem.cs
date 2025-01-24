using System.Threading.Tasks;

namespace LostKaiju.Services.Saves
{
    public interface ISaveSystem
    {
        Task SaveAsync<T>(string key, T data);
        Task<T> LoadAsync<T>(string key);
        Task DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);
    }
}