using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap;
using LostKaiju.Infrastructure.SceneBootstrap.Context;

namespace LostKaiju.Infrastructure.Loading
{
    public class SceneLoader
    {
        private readonly MonoBehaviour _monoHook;
        private readonly LoadingScreen _loadingScreen;
        private const float FAKE_LOAD_TIME = 0.2f;

        public SceneLoader(MonoBehaviour hook, LoadingScreen loadingScreen)
        {
            _monoHook = hook;
            _loadingScreen = loadingScreen;
        }

        public IEnumerator LoadStartScene()
        {
            yield return LoadMainMenu();
        }

        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext = null)
        {
            _loadingScreen.Show();
            yield return new WaitForSeconds(FAKE_LOAD_TIME);
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.MAIN_MENU);

            Debug.Log("Main menu scene loaded");

            var mainMenuBootstrap = Object.FindAnyObjectByType<MainMenuBootstrap>();
            var exitMainMenuSignal = mainMenuBootstrap.Boot(mainMenuEnterContext);

            exitMainMenuSignal.Subscribe(mainMenuExitContext =>
            {
                _monoHook.StartCoroutine(LoadHub(mainMenuExitContext.HubEnterContext));
            });
            _loadingScreen.Hide();
        }

        private IEnumerator LoadHub(HubEnterContext hubEnterContext)
        {
            _loadingScreen.Show();
            yield return new WaitForSeconds(FAKE_LOAD_TIME);
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.HUB);

            Debug.Log("Hub scene loaded");

            var hubExitSignal = Object.FindAnyObjectByType<HubBootstrap>().Boot(hubEnterContext);
            hubExitSignal.Subscribe(hubExitContext =>
            {
                var toSceneName = hubExitContext.ToSceneContext.SceneName;

                if (toSceneName == Scenes.MAIN_MENU)
                {
                    _monoHook.StartCoroutine(LoadMainMenu(hubExitContext.ToSceneContext as MainMenuEnterContext));
                }
                else if (toSceneName == Scenes.GAMEPLAY)
                {
                    _monoHook.StartCoroutine(LoadGameplay(hubExitContext.ToSceneContext as GameplayEnterContext));
                }
            });
            _loadingScreen.Hide();
        }

        private IEnumerator LoadGameplay(GameplayEnterContext gameplayEnterContext)
        {
            _loadingScreen.Show();
            yield return new WaitForSeconds(FAKE_LOAD_TIME);
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.GAMEPLAY);

            Debug.Log("Gameplay scene loaded");
        
            var gameplayBootstrap = Object.FindAnyObjectByType<GameplayBootstrap>();
            var gameplayExitSignal = gameplayBootstrap.Boot(gameplayEnterContext);

            gameplayExitSignal.Subscribe(gameplayExitContext =>
            {
                _monoHook.StartCoroutine(LoadHub(gameplayExitContext.HubEnterContext));
            });

            var levelSceneName = gameplayEnterContext.LevelSceneName;

            yield return LoadSceneAsync(levelSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelSceneName));
            var levelBootstrap = Object.FindAnyObjectByType<MissionBootstrap>();

            levelBootstrap.Boot(gameplayEnterContext);

            Debug.Log($"{gameplayEnterContext.LevelSceneName} scene loaded additively");
            _loadingScreen.Hide();
        }

        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode=LoadSceneMode.Single)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, mode);
        }
    }
}
