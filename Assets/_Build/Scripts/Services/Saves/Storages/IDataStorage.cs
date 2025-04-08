namespace LostKaiju.Services.Saves
{
    public interface IDataStorage
    {
        public void Write(string key, string serializedData);
        public string Read(string key);
        public void Delete(string key);
        public bool Exists(string key);
    }
}