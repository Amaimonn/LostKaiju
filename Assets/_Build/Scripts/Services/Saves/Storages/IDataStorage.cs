using System.Threading.Tasks;

namespace LostKaiju.Services.Saves
{
    public interface IDataStorage
    {
        public Task WriteAsync(string key, string serializedData);
        public Task<string> ReadAsync(string key);
        public Task DeleteAsync(string key);
        public Task<bool> ExistsAsync(string key);
    }
}