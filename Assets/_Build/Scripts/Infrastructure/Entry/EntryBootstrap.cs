using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

using LostKaiju.Infrastructure.Entry.Context;
using LostKaiju.Models.UI.MVVM;
using LostKaiju.Models.Locator;

namespace LostKaiju.Infrastructure.Entry
{
    /// <summary>
    /// Scene bootstrap to initialize the game. Runs only once at the beginning.
    /// </summary>
    public class EntryBootstrap : MonoBehaviour
    {
        [SerializeField] private UIRootBinder _uiRootBinderPrefab;
        private MonoBehaviourHook _monoHook;
        
        public void Boot()
        {
            _monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(_monoHook);

            var uiRootBinder = Instantiate(_uiRootBinderPrefab);
            ServiceLocator.Current.Register(uiRootBinder);
            DontDestroyOnLoad(uiRootBinder);

            var sceneLoader = new SceneLoader(_monoHook);
            _monoHook.StartCoroutine(sceneLoader.LoadStartScene());
        }

        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext = null)
        {
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.MAIN_MENU);

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
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.HUB);

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
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.GAMEPLAY);

            Debug.Log("Gameplay scene loaded");
        
            var gameplayBootstrap = FindFirstObjectByType<GameplayBootstrap>();
            var gameplayExitSignal = gameplayBootstrap.Boot(gameplayEnterContext);
            gameplayExitSignal.Subscribe(gameplayExitContext =>
            {
                _monoHook.StartCoroutine(LoadHub(gameplayExitContext.HubEnterContext));
            });

            yield return SceneManager.LoadSceneAsync(gameplayEnterContext.LevelSceneName, LoadSceneMode.Additive);
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
