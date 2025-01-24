using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace LostKaiju.Services.Saves
{
    public class BinaryAsyncSerializer : IAsyncSerializer
    {
        private readonly BinaryFormatter _formatter = new();
        
        public Task<string> SerializeAsync<T>(T rawData)
        {
            MemoryStream stream = new();

            _formatter.Serialize(stream, rawData);

            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            string serializedData = Encoding.UTF8.GetString(buffer);

            return Task.FromResult(serializedData);
        }

        public Task<T> DeserializeAsync<T>(string serializedData)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(serializedData);

            MemoryStream stream = new(buffer);

            var data = (T)_formatter.Deserialize(stream);
            return Task.FromResult(data);
        }
    }
}