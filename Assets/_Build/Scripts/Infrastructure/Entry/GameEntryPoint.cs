using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

using LostKaiju.Boilerplates.Locator;
using LostKaiju.Services.Inputs;
using LostKaiju.Infrastructure.SceneBootstrap;
using LostKaiju.Services.Saves;
using LostKaiju.Game.Providers.GameState;

namespace LostKaiju.Infrastructure.Entry
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        private GameEntryPoint()
        {
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void EnterTheGame()
        {
            _instance = new();
            await _instance.Run();
        }

        private async Task Run()
        {
            await SceneManager.LoadSceneAsync(Scenes.ENTRY_POINT);
            Debug.Log("Entry point scene loaded");
            
            // Object.FindAnyObjectByType<EntryBootstrap>().Boot();
        }
    }
}
