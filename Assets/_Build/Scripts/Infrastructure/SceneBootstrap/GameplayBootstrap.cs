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

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    /// <summary>
    /// Common dependencies for the Misisons (gameplay scenes).
    /// </summary>
    public class GameplayBootstrap : LifetimeScope
    {
        [SerializeField] private GameplayView _gameplayViewPrefab;
# if MOBILE_BUILD || UNITY_EDITOR
        [SerializeField] private MobileControlsView _mobileControlsViewPrefab;
# endif
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputStateProvider>(Lifetime.Singleton);
            builder.Register<Subject<Unit>>(Lifetime.Singleton);
            builder.Register<TypedRegistration<GameplayExitContext, Subject<Unit>>>(Lifetime.Singleton);
        }

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
        {
            var exitToHubSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;
            var hubEnterContext = new HubEnterContext();
            var gameplayExitContext = new GameplayExitContext(hubEnterContext);
            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var inputStateProvider = Container.Resolve<InputStateProvider>();
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var exitPopUpBinder = new ExitPopUpBinder(rootUIBinder, exitToHubSignal);
            var optionsBinder  = new OptionsBinder(rootUIBinder, inputStateProvider, settingsBinder, exitPopUpBinder);
            var gameplayViewModel = new GameplayViewModel(optionsBinder);
            var gameplayView = Instantiate(_gameplayViewPrefab);

            gameplayView.Bind(gameplayViewModel);
            rootUIBinder.SetView(gameplayView);

# if MOBILE_BUILD || UNITY_EDITOR
            var mobileControls = Instantiate(_mobileControlsViewPrefab);
            rootUIBinder.AddView(mobileControls);
# endif
            var exitGameplaySignal = new Subject<GameplayExitContext>();
            exitToHubSignal.Take(1).Subscribe(_ => 
            {
                Debug.Log("Gameplay exit signal");
                exitGameplaySignal.OnNext(gameplayExitContext);
            });
            return exitGameplaySignal;
        }
    }
}
