using LostKaiju.Boilerplates.Locator;

namespace LostKaiju.Services.Saves
{
    public interface ISaveSystem : IService
    {
        public void Save<T>(string key, T data);
        public T Load<T>(string key);
        public void Delete(string key);
        public bool Exists(string key);
    }
}