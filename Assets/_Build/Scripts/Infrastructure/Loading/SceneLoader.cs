using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap;
using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.Constants;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

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

                Debug.Log("Main menu scene loaded");

                var mainMenuBootstrap = Object.FindAnyObjectByType<MainMenuBootstrap>();
                mainMenuBootstrap.Build();
                var exitMainMenuSignal = mainMenuBootstrap.Boot(mainMenuEnterContext);

                exitMainMenuSignal.Take(1).Subscribe(mainMenuExitContext =>
                {
                    _monoHook.StartCoroutine(LoadHub(mainMenuExitContext.HubEnterContext));
                });
            }
            yield return GetRemainFakeLoadTime(startTime);
            yield return _loadingScreen.HideCoroutine();
        }

        private IEnumerator LoadHub(HubEnterContext hubEnterContext)
        {
            yield return _loadingScreen.ShowCoroutine();
            var startTime = Time.time;

            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.HUB);

                Debug.Log("Hub scene loaded");

                var hubBootstrap = Object.FindAnyObjectByType<HubBootstrap>();
                hubBootstrap.Build();

                var hubExitSignalTask = hubBootstrap.BootAsync(hubEnterContext);
                yield return new WaitUntil(() => hubExitSignalTask.IsCompleted);
                var hubExitSignal = hubExitSignalTask.Result;
                
                hubExitSignal.Take(1).Subscribe(hubExitContext =>
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
            yield return GetRemainFakeLoadTime(startTime);
            yield return _loadingScreen.HideCoroutine();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private IEnumerator LoadGameplay(GameplayEnterContext gameplayEnterContext)
        {
            yield return _loadingScreen.ShowCoroutine();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            var startTime = Time.time;

            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.GAMEPLAY);

                Debug.Log("Gameplay scene loaded");

                var gameplayBootstrap = Object.FindAnyObjectByType<GameplayBootstrap>();
                gameplayBootstrap.Build();

                var gameplayExitSignalTask = gameplayBootstrap.BootAsync(gameplayEnterContext);
                yield return new WaitUntil(() => gameplayExitSignalTask.IsCompleted);
                var gameplayExitSignal = gameplayExitSignalTask.Result;

                gameplayExitSignal.Take(1).Subscribe(gameplayExitContext =>
                {
                    _monoHook.StartCoroutine(LoadHub(gameplayExitContext.HubEnterContext));
                });

                var levelSceneName = gameplayEnterContext.LevelSceneName;
                var missionEnterContextStub = new MissionEnterContext(gameplayEnterContext);
                yield return LoadMissionAdditive(gameplayBootstrap, missionEnterContextStub,
                    toMissionSceneName: levelSceneName);
            }
            
            yield return GetRemainFakeLoadTime(startTime);
            yield return _loadingScreen.HideCoroutine();
        }

        private IEnumerator LoadMissionAdditive(LifetimeScope parentScope, MissionEnterContext missionEnterContext,
            string toMissionSceneName, AsyncOperationHandle<SceneInstance>? fromMissionSceneHandle = null)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.GAMEPLAY));
            if (fromMissionSceneHandle != null)
            {
                yield return UnloadSceneAsync(fromMissionSceneHandle.Value);
            }

            using (LifetimeScope.EnqueueParent(parentScope))
            {
                var toSceneHandle = LoadSceneAsync(toMissionSceneName, LoadSceneMode.Additive);
                yield return toSceneHandle;

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(toMissionSceneName));

                var missionBootstrap = Object.FindAnyObjectByType<MissionBootstrap>();
                missionBootstrap.Build();
                var missionExitSignal = missionBootstrap.Boot(missionEnterContext);
                missionExitSignal.Take(1).Subscribe(missionExitContext =>
                {
                    _loadingScreen.Show();

                    var toSceneName = missionExitContext.ToMissionSceneName;
                    var toSceneContext = missionExitContext.MissionEnterContext;
                    _monoHook.StartCoroutine(LoadMissionAdditive(parentScope, toSceneContext, toMissionSceneName: toSceneName,
                        fromMissionSceneHandle: toSceneHandle));

                    _loadingScreen.Hide();
                });
            }
        }

        private AsyncOperationHandle<SceneInstance> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return Addressables.LoadSceneAsync($"Scenes/{sceneName}", mode);
        }

        private IEnumerator UnloadSceneAsync(AsyncOperationHandle<SceneInstance> sceneHandle)
        {
            yield return Addressables.UnloadSceneAsync(sceneHandle);
        }

        private YieldInstruction GetRemainFakeLoadTime(float startTime)
        {
            var currentTime = Time.time;
            var remainTime = FAKE_LOAD_TIME - (currentTime - startTime);
            if (remainTime > 0)
                return new WaitForSeconds(remainTime);
            else
                return null;
        }
    }
}
