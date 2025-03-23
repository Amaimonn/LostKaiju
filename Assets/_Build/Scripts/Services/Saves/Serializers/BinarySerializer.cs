using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace LostKaiju.Services.Saves
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryFormatter _formatter = new();
        
        public string Serialize<T>(T rawData)
        {
            MemoryStream stream = new();

            _formatter.Serialize(stream, rawData);

            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            string serializedData = Encoding.UTF8.GetString(buffer);

            return serializedData;
        }

        public T Deserialize<T>(string serializedData)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(serializedData);

            MemoryStream stream = new(buffer);

            var data = (T)_formatter.Deserialize(stream);
            return data;
        }
    }
}