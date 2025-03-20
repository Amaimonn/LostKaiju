using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
using LostKaiju.Game.Constants; // <-- used with web/mobile builds

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    /// <summary>
    /// Common dependencies for the Misisons (gameplay scenes).
    /// </summary>
    public class GameplayBootstrap : LifetimeScope
    {
        [SerializeField] private GameplayView _gameplayViewPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputStateProvider>(Lifetime.Singleton);
            builder.Register<TypedRegistration<GameplayExitContext, Subject<Unit>>>(Lifetime.Singleton);
            builder.Register<ExitPopUpBinder>(x => new ExitPopUpBinder(x.Resolve<IRootUIBinder>(), 
                x.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance), Lifetime.Singleton);
            builder.Register<OptionsBinder>(Lifetime.Singleton);
        }

        public async Task<Observable<GameplayExitContext>> BootAsync(GameplayEnterContext gameplayEnterContext)
        {
            var playerConfigHandle = Addressables.LoadAssetAsync<IPlayerConfig>(gameplayEnterContext.PlayerConfigPath);
            await playerConfigHandle.Task;
            gameplayEnterContext.PlayerConfig = playerConfigHandle.Result;

            var exitGameplaySignal = new Subject<GameplayExitContext>();
            var exitToHubSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;
            var hubEnterContext = new HubEnterContext()
            {
                ExitingMissionId = gameplayEnterContext.SelectedMissionData.Id
            };
            
            var gameplayExitContext = new GameplayExitContext(hubEnterContext);
            gameplayEnterContext.MissionCompletionSignal.Take(1).Subscribe(_ =>
            {
                hubEnterContext.IsMissionCompleted = true;
            });

            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var inputProvider = Container.Resolve<IInputProvider>();
            var optionsBinder  =  Container.Resolve<OptionsBinder>();
            var loadingNotifier = Container.Resolve<ILoadingScreenNotifier>();
            loadingNotifier.OnFinished.Take(1).Subscribe(_ => optionsBinder.BindEscapeSignal(inputProvider.OnEscape));
            var cursorDisposable = optionsBinder.OnOpened.Subscribe(x => 
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                x.OnClosingCompleted.Take(1).Subscribe(_ =>
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                });
            });
            var gameplayViewModel = new GameplayViewModel(optionsBinder);
            var gameplayView = Instantiate(_gameplayViewPrefab);

            gameplayView.Bind(gameplayViewModel);
            rootUIBinder.SetView(gameplayView);

# if MOBILE_BUILD || WEB_BUILD
           if (SystemInfo.deviceType == DeviceType.Handheld)
            {   
                var mobileControlsViewPrefab = Resources.Load<MobileControlsView>(Paths.MOBILE_CONTROLS_VIEW);
                var mobileControls = Instantiate(mobileControlsViewPrefab);
                var inputStateProvider = Container.Resolve<InputStateProvider>();
                inputStateProvider.IsInputEnabled.Subscribe(x => mobileControls.gameObject.SetActive(x));
                rootUIBinder.AddView(mobileControls);
            }
# endif
            exitToHubSignal.Take(1).Subscribe(_ => 
            {
                Debug.Log("Gameplay exit signal");
                cursorDisposable.Dispose();
                optionsBinder.Dispose(); // shouldn`t be opened with a loading screen by clicking escape.
                exitGameplaySignal.OnNext(gameplayExitContext);
            });

            return exitGameplaySignal;
        }
    }
}
