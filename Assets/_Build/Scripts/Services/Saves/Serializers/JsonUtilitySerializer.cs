using UnityEngine;

namespace LostKaiju.Services.Saves
{
    public class JsonUtilitySerializer : ISerializer
    {
        public string Serialize<T>(T rawData)
        {
            return JsonUtility.ToJson(rawData);
        }

        public T Deserialize<T>(string serializedData)
        {
            return JsonUtility.FromJson<T>(serializedData);
        }
    }
}