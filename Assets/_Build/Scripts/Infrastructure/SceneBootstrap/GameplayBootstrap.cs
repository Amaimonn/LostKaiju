using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.Player.Data.Models;
using LostKaiju.Game.UI.MVVM.Gameplay.MobileControls;
using LostKaiju.Infrastructure.SceneBootstrap.Context;

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

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
        {
            var exitGameplaySignal = new Subject<GameplayExitContext>();
            var uiRootBinder = Container.Resolve<IRootUIBinder>();
            var playerLiveParameters = new PlayerLiveParametersModel();
            var gameplayViewModel = new GameplayViewModel(playerLiveParameters);
            var gameplayView = Instantiate(_gameplayViewPrefab);

            gameplayView.Bind(gameplayViewModel);
            uiRootBinder.SetView(gameplayView);

# if MOBILE_BUILD || UNITY_EDITOR
            var mobileControls = Instantiate(_mobileControlsViewPrefab);
            uiRootBinder.AddView(mobileControls);
# endif
            return exitGameplaySignal;
        }
    }
}
