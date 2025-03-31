using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer.Unity;
using R3;
using YG;

using LostKaiju.Infrastructure.SceneBootstrap;
using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.Constants;

namespace LostKaiju.Infrastructure.Loading
{
    public class SceneLoader
    {
        public Observable<Unit> OnLoadingStarted => _onLoadingStarted;
        public Observable<Unit> OnLoadingFinished => _onLoadingFinished;
        
        private readonly MonoBehaviour _monoHook;
        private readonly LoadingScreen _loadingScreen;
        private readonly LifetimeScope _rootScope;
        private readonly Subject<Unit> _onLoadingStarted = new();
        private readonly Subject<Unit> _onLoadingFinished = new();
        private const float MIN_LOADING_TIME = 1f;

        public SceneLoader(MonoBehaviour hook, LoadingScreen loadingScreen, LifetimeScope rootScope)
        {
            _monoHook = hook;
            _loadingScreen = loadingScreen;
            _rootScope = rootScope;
        }

        public IEnumerator LoadStartScene()
        {
            yield return LoadHub(fastLoadingShow: true);
        }

        private IEnumerator LoadMainMenu(MainMenuEnterContext mainMenuEnterContext = null)
        {
            _loadingScreen.Show();
            var startTime = Time.time;
            _onLoadingStarted.OnNext(Unit.Default);

            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.MAIN_MENU);
                _onLoadingFinished.OnNext(Unit.Default);

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

        private IEnumerator LoadHub(HubEnterContext hubEnterContext = null, bool fastLoadingShow = false)
        {
            if (fastLoadingShow)
                _loadingScreen.Show();
            else
                yield return _loadingScreen.ShowCoroutine();
            var startTime = Time.time;

            _onLoadingStarted.OnNext(Unit.Default);
            yield return LoadSceneAsync(Scenes.GAP);
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.HUB);
                _onLoadingFinished.OnNext(Unit.Default);

                Debug.Log("Hub scene loaded");

                var hubBootstrap = Object.FindAnyObjectByType<HubBootstrap>();
                hubBootstrap.Build();
                var hubExitSignal = hubBootstrap.Boot(hubEnterContext);
                
                hubExitSignal.Take(1).Subscribe(hubExitContext =>
                {
                    var toSceneName = hubExitContext.ToSceneContext.SceneName;

                    if (toSceneName == Scenes.GAMEPLAY)
                    {
                        _monoHook.StartCoroutine(LoadGameplay(hubExitContext.ToSceneContext as GameplayEnterContext));
                    }
                    // else if (toSceneName == Scenes.MAIN_MENU)
                    // {
                    //     _monoHook.StartCoroutine(LoadMainMenu(hubExitContext.ToSceneContext as MainMenuEnterContext));
                    // }
                });
            }
            yield return GetRemainFakeLoadTime(startTime);
            yield return _loadingScreen.HideCoroutine();
        }

        private IEnumerator LoadGameplay(GameplayEnterContext gameplayEnterContext)
        {
            yield return _loadingScreen.ShowCoroutine();
            
            var startTime = Time.time;
            _onLoadingStarted.OnNext(Unit.Default);
            ShowAdvertising();
            yield return LoadSceneAsync(Scenes.GAP);

            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                yield return LoadSceneAsync(Scenes.GAMEPLAY);

                Debug.Log("Gameplay scene loaded");

                var gameplayBootstrap = Object.FindAnyObjectByType<GameplayBootstrap>();
                gameplayBootstrap.Build();
                var gameplayExitSignal = gameplayBootstrap.Boot(gameplayEnterContext);

                gameplayExitSignal.Take(1).Subscribe(gameplayExitContext =>
                {
                    _monoHook.StartCoroutine(LoadHub(gameplayExitContext.HubEnterContext));
                });

                var levelSceneName = gameplayEnterContext.LevelSceneName;
                var missionEnterContextStub = new MissionEnterContext(gameplayEnterContext);
                yield return LoadMissionAdditive(gameplayBootstrap, missionEnterContextStub,
                    toMissionSceneName: levelSceneName);
                _onLoadingFinished.OnNext(Unit.Default);
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
                    var toSceneName = missionExitContext.ToMissionSceneName;
                    var toSceneContext = missionExitContext.MissionEnterContext;
                    _monoHook.StartCoroutine(MissionSceneTransition());
                    IEnumerator MissionSceneTransition()
                    {
                        _loadingScreen.Show();
                        _onLoadingStarted.OnNext(Unit.Default);

                        yield return LoadMissionAdditive(parentScope, toSceneContext, toMissionSceneName: toSceneName,
                            fromMissionSceneHandle: toSceneHandle);

                        _onLoadingFinished.OnNext(Unit.Default);
                        _loadingScreen.Hide();
                    }
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
            var remainTime = MIN_LOADING_TIME - (currentTime - startTime);
            if (remainTime > 0)
                return new WaitForSeconds(remainTime);
            else
                return null;
        }

        private void ShowAdvertising()
        {
# if YG_BUILD
            YG2.InterstitialAdvShow();
#endif
        }
    }
}
