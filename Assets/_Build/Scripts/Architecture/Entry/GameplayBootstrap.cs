using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;
using LostKaiju.Architecture.Services;
using LostKaiju.UI.MVVM;
using LostKaiju.UI.MVVM.Gameplay;
using LostKaiju.Player.Data.Model;

namespace LostKaiju.Architecture.Entry
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
            var uiRootBinder = ServiceLocator.Current.Get<UIRootBinder>();
            var playerLiveParameters = new PlayerLiveParametersModel();
            var gameplayViewModel = new GameplayViewModel(playerLiveParameters);
            var gameplayView = Instantiate(_gameplayViewPrefab).GetComponent<GameplayView>();

            gameplayView.Bind(gameplayViewModel);
            uiRootBinder.SetView(gameplayView);

# if MOBILE_BUILD || UNITY_EDITOR
            var mobileControls = Instantiate(_mobileControlsPrefab);
            uiRootBinder.Attach(mobileControls); // TODO: add view for mobile controls and use SetViews method
# endif
            return exitGameplaySignal;
        }
    }
}
