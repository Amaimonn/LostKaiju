using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap;
using LostKaiju.Infrastructure.SceneBootstrap.Context;

namespace LostKaiju.Infrastructure.Loading
{
    public class SceneLoader
    {
        private readonly MonoBehaviour _monoHook;
        private readonly LoadingScreen _loadingScreen;
        private readonly LifetimeScope _rootScope;
        private const float FAKE_LOAD_TIME = 0.1f;

        public SceneLoader(MonoBehaviour hook, LoadingScreen loadingScreen, LifetimeScope rootScope)
        {
            _monoHook = hook;
            _loadingScreen = loadingScreen;
            _rootScope = rootScope;
        }

        public IEnumerator LoadStartScene()
        {
            yield return LoadMainMenu();
        }

        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext = null)
        {
            _loadingScreen.Show();
            var startTime = Time.time;

            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.MAIN_MENU);
            }

            Debug.Log("Main menu scene loaded");

            var mainMenuBootstrap = Object.FindAnyObjectByType<MainMenuBootstrap>();
            var exitMainMenuSignal = mainMenuBootstrap.Boot(mainMenuEnterContext);

            exitMainMenuSignal.Subscribe(mainMenuExitContext =>
            {
                _monoHook.StartCoroutine(LoadHub(mainMenuExitContext.HubEnterContext));
            });

            yield return GetRemainFakeLoadTime(startTime);
            _loadingScreen.Hide();
        }

        private IEnumerator LoadHub(HubEnterContext hubEnterContext)
        {
            _loadingScreen.Show();
            var startTime = Time.time;

            var wait = new WaitForSeconds(FAKE_LOAD_TIME);
            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.HUB);
            }
            Debug.Log("Hub scene loaded");

            var hubBootstrap = Object.FindAnyObjectByType<HubBootstrap>();
            var hubExitSignal = hubBootstrap.Boot(hubEnterContext);

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

            yield return GetRemainFakeLoadTime(startTime);
            _loadingScreen.Hide();
        }

        private IEnumerator LoadGameplay(GameplayEnterContext gameplayEnterContext)
        {
            _loadingScreen.Show();
            var startTime = Time.time;

            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.GAMEPLAY);
            }
            Debug.Log("Gameplay scene loaded");

            var gameplayBootstrap = Object.FindAnyObjectByType<GameplayBootstrap>();
            var gameplayExitSignal = gameplayBootstrap.Boot(gameplayEnterContext);

            gameplayExitSignal.Subscribe(gameplayExitContext =>
            {
                _monoHook.StartCoroutine(LoadHub(gameplayExitContext.HubEnterContext));
            });

            var levelSceneName = gameplayEnterContext.LevelSceneName;
            var missionEnterContextStub = new MissionEnterContext(gameplayEnterContext);
            yield return LoadMissionAdditive(gameplayBootstrap, missionEnterContextStub, 
                toMissionSceneName: levelSceneName);
                
            yield return GetRemainFakeLoadTime(startTime);
            _loadingScreen.Hide();
        }

        private IEnumerator LoadMissionAdditive(LifetimeScope parentScope, MissionEnterContext missionEnterContext, 
            string toMissionSceneName, string fromMissionSceneName=null)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.GAMEPLAY));
            if (fromMissionSceneName != null)
            {
                yield return UnloadSceneAsync(fromMissionSceneName);
            }

            using (LifetimeScope.EnqueueParent(parentScope))
            {
                yield return LoadSceneAsync(toMissionSceneName, LoadSceneMode.Additive);
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(toMissionSceneName));

            var missionBootstrap = Object.FindAnyObjectByType<MissionBootstrap>();
            var missionExitSignal = missionBootstrap.Boot(missionEnterContext);
            missionExitSignal.Subscribe(missionExitContext =>
            {
                _loadingScreen.Show();

                var toSceneName = missionExitContext.MissionEnterSceneName;
                var toSceneContext = missionExitContext.MissionEnterContext;
                _monoHook.StartCoroutine(LoadMissionAdditive(parentScope, toSceneContext, toMissionSceneName : toSceneName, 
                    fromMissionSceneName: toMissionSceneName));

                _loadingScreen.Hide();
            });
        }   

        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode=LoadSceneMode.Single)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, mode);
        }

        private IEnumerator UnloadSceneAsync(string sceneName)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }

        private YieldInstruction GetRemainFakeLoadTime(float startTime)
        {
            var currentTime = Time.time;
            var remainTime = FAKE_LOAD_TIME - (currentTime - startTime);
            if (remainTime > 0)
            {
                return new WaitForSeconds(remainTime);
            }
            else
            {   
                return new WaitForSeconds(0);
            }
        }
    }
}
