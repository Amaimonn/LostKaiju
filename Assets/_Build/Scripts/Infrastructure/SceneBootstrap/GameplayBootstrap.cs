using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.UI.MVVM.Gameplay.MobileControls;
using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.Providers.InputState;
using LostKaiju.Infrastructure.Scopes;
using LostKaiju.Services.Inputs;
using LostKaiju.Infrastructure.Loading;
using LostKaiju.Game.World.Player.Data.Configs;
using LostKaiju.Game.Constants;
using LostKaiju.Services.Audio;
using UnityEngine.AddressableAssets;
using LostKaiju.Game.UI.MVVM.Gameplay.MissionResults;
using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    /// <summary>
    /// Common dependencies for the Misisons (gameplay scenes).
    /// </summary>
    public class GameplayBootstrap : LifetimeScope
    {
        [SerializeField] private GameplayView _gameplayViewPrefab;

        private GameplayEnterContext _gameplayEnterContext;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputStateProvider>(Lifetime.Singleton);
            builder.Register<TypedRegistration<GameplayExitContext, Subject<Unit>>>(Lifetime.Singleton);
            builder.Register<TypedRegistration<MissionResultsParameters, Subject<Unit>>>(Lifetime.Singleton);
            builder.Register<ExitPopUpBinder>(x => 
                new ExitPopUpBinder(x.Resolve<IRootUIBinder>(), 
                    x.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance, 
                    x.Resolve<AudioPlayer>()), 
                Lifetime.Singleton);
            builder.Register<OptionsBinder>(Lifetime.Singleton);
            builder.Register<MissionResultsParameters>(Lifetime.Singleton);
            builder.Register<MissionResultsBinder>(x => 
                new MissionResultsBinder(x.Resolve<IRootUIBinder>(),
                    x.Resolve<AudioPlayer>(),
                    _gameplayEnterContext.SelectedLocationData,
                    _gameplayEnterContext.SelectedMissionData), 
                Lifetime.Singleton);
        }

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext, out Observable<bool> onLoaded)
        {
            _gameplayEnterContext = gameplayEnterContext;
            Build();
            var onLoadedProperty = new ReactiveProperty<bool>(false);
            onLoaded = onLoadedProperty;
            var playerConfigHandle = Addressables.LoadAssetAsync<PlayerConfigSO>(gameplayEnterContext.PlayerConfigPath);
            playerConfigHandle.Completed += playerConfig =>
            {
                gameplayEnterContext.PlayerConfig = playerConfig.Result;
                onLoadedProperty.Value = true;
            };
            // gameplayEnterContext.PlayerConfig = Resources.Load<PlayerConfigSO>(gameplayEnterContext.PlayerConfigPath);

            var exitGameplaySignal = new Subject<GameplayExitContext>();
            var exitToHubSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;
            var hubEnterContext = new HubEnterContext()
            {
                ExitingMissionId = gameplayEnterContext.SelectedMissionData.Id
            };
            
            var gameplayExitContext = new GameplayExitContext(hubEnterContext);

            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var inputProvider = Container.Resolve<IInputProvider>();
            var optionsBinder  =  Container.Resolve<OptionsBinder>();
            var inputStateProvider = Container.Resolve<InputStateProvider>();
            var loadingNotifier = Container.Resolve<ILoadingScreenNotifier>();
            loadingNotifier.OnFinished.Take(1)
                .Subscribe(_ => optionsBinder.BindEscapeSignal(inputProvider.OnOptions));

            var loadingInputBlocker = new FakeBlocker();
            inputStateProvider.AddBlocker(loadingInputBlocker);
            loadingNotifier.OnStarted.TakeUntil(exitGameplaySignal)
                .Subscribe(_ => inputStateProvider.AddBlocker(loadingInputBlocker));
            loadingNotifier.OnFinished.TakeUntil(exitGameplaySignal)
                .Subscribe(_ => inputStateProvider.RemoveBlocker(loadingInputBlocker));

            var gameplayViewModel = new GameplayViewModel(optionsBinder);
            var gameplayView = Instantiate(_gameplayViewPrefab);

            gameplayView.Bind(gameplayViewModel);
            rootUIBinder.SetView(gameplayView);
            var missionResultsBinder = Container.Resolve<MissionResultsBinder>();
            var completionSignal = Container.Resolve<TypedRegistration<MissionResultsParameters, Subject<Unit>>>().Instance;
            completionSignal.Take(1).Subscribe(_ => 
            {
                optionsBinder.CloseAll();
                optionsBinder.Dispose();
                
                var results = Container.Resolve<MissionResultsParameters>();
                hubEnterContext.IsMissionCompleted = true;
                missionResultsBinder.SetResults(results);
                // TODO: PopUp logic with GameplayView
                // gameplayViewModel.OpenMissionCompletionPopUp();
                if (missionResultsBinder.TryBindAndOpen(out var resultsViewModel))
                {
                    resultsViewModel.OnOpenStateChanged.Where(isOpened => isOpened == false)
                        .Take(1)
                        .Subscribe(_ => exitToHubSignal.OnNext(Unit.Default));
                }
                gameplayEnterContext.MissionCompletionSignal.OnNext(results);
            });
            
# if WEB_BUILD && YG_BUILD
           if(YG.YG2.envir.isMobile || YG.YG2.envir.isTablet)
            {   
                var mobileControlsViewPrefab = Resources.Load<MobileControlsView>(Paths.MOBILE_CONTROLS_VIEW);
                var mobileControls = Instantiate(mobileControlsViewPrefab);
                // var inputStateProvider = Container.Resolve<InputStateProvider>();
                inputStateProvider.IsInputEnabled.Subscribe(x => mobileControls.gameObject.SetActive(x));
                rootUIBinder.AddView(mobileControls);
            }
# endif
            exitToHubSignal.Take(1).Subscribe(_ => 
            {
                inputStateProvider.AddBlocker(loadingInputBlocker);
                Debug.Log("Gameplay exit signal");
                optionsBinder.Dispose(); // shouldn`t be opened with a loading screen by clicking escape.
                exitGameplaySignal.OnNext(gameplayExitContext);
            });

            return exitGameplaySignal;
        }
    }
}
