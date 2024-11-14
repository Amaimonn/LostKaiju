using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    /// <summary>
    /// Scene bootstrap to initialize the game. Runs only once at the beginning.
    /// </summary>
    public class EntryBootstrap : MonoBehaviour
    {
        private MonoBehaviourHook _monoHook;
        
        public void Boot()
        {
            // Some initialization code here
            _monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(_monoHook);

            _monoHook.StartCoroutine(LoadMainMenu());
        }

        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext = null)
        {
            yield return SceneManager.LoadSceneAsync(Scenes.GAP);
            yield return SceneManager.LoadSceneAsync(Scenes.MAIN_MENU);

            Debug.Log("Main menu scene loaded");

            var mainMenuBootstrap = FindFirstObjectByType<MainMenuBootstrap>();
            var exitMainMenuSignal = mainMenuBootstrap.Boot(mainMenuEnterContext);
            exitMainMenuSignal.Subscribe(mainMenuExitContext =>
            {
                _monoHook.StartCoroutine(LoadHub(mainMenuExitContext.HubEnterContext));
            });
        }

        private IEnumerator LoadHub(HubEnterContext hubEnterContext)
        {
            yield return SceneManager.LoadSceneAsync(Scenes.GAP);
            yield return SceneManager.LoadSceneAsync(Scenes.HUB);

            Debug.Log("Hub scene loaded");

            var hubExitSignal = FindFirstObjectByType<HubBootstrap>().Boot(hubEnterContext);
            hubExitSignal.Subscribe(hubExitContext =>
            {
                var toScene = hubExitContext.ToSceneContext.SceneName;

                if (toScene == Scenes.MAIN_MENU)
                {
                    _monoHook.StartCoroutine(LoadMainMenu(hubExitContext.ToSceneContext as MainMenuEnterContext));
                }
                else if (toScene == Scenes.GAMEPLAY)
                {
                    _monoHook.StartCoroutine(LoadGameplay(hubExitContext.ToSceneContext as GameplayEnterContext));
                }
            });
        }

        private IEnumerator LoadGameplay(GameplayEnterContext gameplayEnterContext)
        {
            yield return SceneManager.LoadSceneAsync(Scenes.GAP);
            yield return SceneManager.LoadSceneAsync(Scenes.GAMEPLAY);

            Debug.Log("Gameplay scene loaded");
        
            var gameplayBootstrap = FindFirstObjectByType<GameplayBootstrap>();
            var gameplayExitSignal = gameplayBootstrap.Boot(gameplayEnterContext);
            gameplayExitSignal.Subscribe(gameplayExitContext =>
            {
                StartCoroutine(LoadHub(gameplayExitContext.HubEnterContext));
            });
        }
    }
}
