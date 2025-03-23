namespace LostKaiju.Services.Saves
{
    public interface ISerializer
    {
        public string Serialize<T>(T rawData);
        public T Deserialize<T>(string serializedData);
    }
}    