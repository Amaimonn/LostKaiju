using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

using LostKaiju.Infrastructure.Entry.Context;

namespace LostKaiju.Infrastructure.Entry
{
    public class SceneLoader
    {
        private MonoBehaviour _monoHook;

        public SceneLoader(MonoBehaviour hook)
        {
            _monoHook = hook;
        }

        public IEnumerator LoadStartScene()
        {
            yield return LoadMainMenu();
        }

        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext = null)
        {
            yield return LoadSceneAsync(Scenes.GAP);
            yield return LoadSceneAsync(Scenes.MAIN_MENU);

            Debug.Log("Main menu scene loaded");

            var mainMenuBootstrap = Object.FindAnyObjectByType<MainMenuBootstrap>();
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
        }

        private IEnumerator LoadGameplay(GameplayEnterContext gameplayEnterContext)
        {
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

            var levelBootstrap = Object.FindAnyObjectByType<LevelBootstrap>();

            levelBootstrap.Boot(gameplayEnterContext);

            Debug.Log($"{gameplayEnterContext.LevelSceneName} scene loaded additively");
        }

        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode=LoadSceneMode.Single)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, mode);
        }
    }
}
