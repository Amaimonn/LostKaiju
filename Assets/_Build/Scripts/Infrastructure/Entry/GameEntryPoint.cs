using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

using LostKaiju.Boilerplates.Locator;
using LostKaiju.Infrastructure.Providers.Inputs;
using LostKaiju.Infrastructure.SceneBootstrap;

namespace LostKaiju.Infrastructure.Entry
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        public GameEntryPoint()
        {
            ServiceLocator.Current.Register<IInputProvider>(new InputSystemProvider());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void EnterTheGame()
        {
            _instance = new();
            await _instance.Run();
        }

        public async Task Run()
        {
            await SceneManager.LoadSceneAsync(Scenes.ENTRY_POINT);
            Debug.Log("Entry point scene loaded");
            Object.FindAnyObjectByType<EntryBootstrap>().Boot();
        }
    }
}
