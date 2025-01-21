using UnityEngine;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.Locator;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.Player.Data.Models;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    /// <summary>
    /// General Bootstrap for gameplay levels set up
    /// </summary>
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private GameplayView _gameplayViewPrefab;
# if MOBILE_BUILD || UNITY_EDITOR
        [SerializeField] private GameObject _mobileControlsPrefab;
# endif

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
        {
            var exitGameplaySignal = new Subject<GameplayExitContext>();
            var uiRootBinder = ServiceLocator.Current.Get<IRootUIBinder>();
            var playerLiveParameters = new PlayerLiveParametersModel();
            var gameplayViewModel = new GameplayViewModel(playerLiveParameters);
            var gameplayView = Instantiate(_gameplayViewPrefab);

            gameplayView.Bind(gameplayViewModel);
            uiRootBinder.SetView(gameplayView);

// # if MOBILE_BUILD || UNITY_EDITOR
//             var mobileControls = Instantiate(_mobileControlsPrefab);
//             uiRootBinder.Attach(mobileControls); // TODO: add view for mobile controls and use SetViews method
// # endif
            return exitGameplaySignal;
        }
    }
}
