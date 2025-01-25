using System.Threading.Tasks;
using UnityEngine;

namespace LostKaiju.Services.Saves
{
    public class JsonUtilitySerializer : ISerializer
    {
        public Task<string> SerializeAsync<T>(T rawData)
        {
            return Task.FromResult(JsonUtility.ToJson(rawData));
        }

        public Task<T> DeserializeAsync<T>(string serializedData)
        {
            return Task.FromResult(JsonUtility.FromJson<T>(serializedData));
        }
    }
}