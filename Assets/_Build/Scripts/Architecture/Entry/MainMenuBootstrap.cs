using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuUI;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var mainMenuExitSignal = new Subject<MainMenuExitContext>();
            mainMenuExitSignal.Subscribe(mainMenuExitContext => LoadHub(mainMenuExitContext)); // send to UI

            //test
            var hubEnterContext = new HubEnterContext();
            var mainMenuExitContext = new MainMenuExitContext(hubEnterContext);
            LoadHub(mainMenuExitContext);
            //end test

            return mainMenuExitSignal;
        }

        private async void LoadHub(MainMenuExitContext mainMenuExitContext)
        {
            await SceneManager.LoadSceneAsync(Scenes.GAP);
            await SceneManager.LoadSceneAsync(Scenes.HUB);

            Debug.Log("Hub scene loaded");

            var hubExitSignal = FindFirstObjectByType<HubBootstrap>().Boot(mainMenuExitContext.HubEnterContext);
            hubExitSignal.Subscribe(hubExitContext =>
            {
                var toScene = hubExitContext.ToSceneContext.SceneName;

                if (toScene == Scenes.MAIN_MENU)
                {
                    StartCoroutine(LoadMainMenu(hubExitContext.ToSceneContext as MainMenuEnterContext));
                }
            });
        }

        // main menu scene can be loaded itself, if it get signal
        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext)
        {
            yield return SceneManager.LoadSceneAsync(Scenes.GAP);
            yield return SceneManager.LoadSceneAsync(Scenes.MAIN_MENU);
            var mainMenuBootstrap = FindFirstObjectByType<MainMenuBootstrap>();
            var exitMainMenuSignal = mainMenuBootstrap.Boot(mainMenuEnterContext);
        }
    }
}
