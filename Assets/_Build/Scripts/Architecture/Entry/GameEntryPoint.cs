using UnityEngine;
using UnityEngine.SceneManagement;

using LostKaiju.Architecture.Providers;
using LostKaiju.Architecture.Services;
using System.Threading.Tasks;

namespace LostKaiju.Architecture.Entry
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        public GameEntryPoint()
        {
            ServiceLocator.Current.Register<IInputProvider>(new BaseInputProvider());
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
            Object.FindFirstObjectByType<EntryBootstrap>().Boot();
        }
    }
}
