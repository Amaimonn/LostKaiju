using System.Threading.Tasks;

namespace LostKaiju.Services.Saves
{
    public interface IDataStorage
    {
        Task WriteAsync(string key, string serializedData);
        Task<string> ReadAsync(string key);
        Task DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);
    }
}