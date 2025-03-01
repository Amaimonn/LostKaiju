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
        }

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
        {
            var exitSignal = new Subject<Unit>();
            var hubEnterContext = new HubEnterContext();
            var gameplayExitContext = new GameplayExitContext(hubEnterContext);
            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var inputStateProvider = Container.Resolve<InputStateProvider>();
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var exitPopUpBinder = new ExitPopUpBinder(rootUIBinder, exitSignal);
            var optionsBinder  = new OptionsBinder(rootUIBinder, inputStateProvider, settingsBinder, exitPopUpBinder);
            // var optionsBinder = Container.Resolve<OptionsBinder>();
            var gameplayViewModel = new GameplayViewModel(optionsBinder);
            var gameplayView = Instantiate(_gameplayViewPrefab);

            gameplayView.Bind(gameplayViewModel);
            rootUIBinder.SetView(gameplayView);

# if MOBILE_BUILD || UNITY_EDITOR
            var mobileControls = Instantiate(_mobileControlsViewPrefab);
            rootUIBinder.AddView(mobileControls);
# endif
            var exitGameplaySignal = exitSignal.Select(_ => gameplayExitContext);
            return exitGameplaySignal;
        }
    }
}
