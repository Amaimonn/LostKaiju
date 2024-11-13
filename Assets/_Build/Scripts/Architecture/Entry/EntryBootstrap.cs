using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    /// <summary>
    /// Scene bootstrap to initialize the game. Runs only once at the beginning.
    /// </summary>
    public class EntryBootstrap : MonoBehaviour
    {
        public void Boot()
        {
            // Some initialization code here
            Debug.Log("Entry scene booted");
            LoadMainMenu();
        }

        private async void LoadMainMenu()
        {
            await SceneManager.LoadSceneAsync(Scenes.GAP);
            await SceneManager.LoadSceneAsync(Scenes.MAIN_MENU);

            Debug.Log("Main menu scene loaded");

            var mainMenuEnterContext = new MainMenuEnterContext();
            var mainMenuBootstrap = FindFirstObjectByType<MainMenuBootstrap>();
            var exitMainMenuSignal = mainMenuBootstrap.Boot(mainMenuEnterContext);
        }
    }
}
