using System.Threading.Tasks;

namespace LostKaiju.Services.Saves
{
    public interface IAsyncSerializer
    {
        public Task<string> SerializeAsync<T>(T rawData);
        public Task<T> DeserializeAsync<T>(string serializedData);
    }
}    