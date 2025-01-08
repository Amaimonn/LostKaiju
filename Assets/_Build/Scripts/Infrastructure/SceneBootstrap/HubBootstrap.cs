using UnityEngine;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Models.Locator;
using LostKaiju.Models.UI.MVVM;
using LostKaiju.Gameplay.UI.MVVM.Hub;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : MonoBehaviour
    {
        [SerializeField] private HubView _hubViewPrefab;
        [SerializeField] private string _playerConfigName; // choose your player character
        
        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var uiRootBinder = ServiceLocator.Current.Get<UIRootBinder>();

            var exitSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext(Scenes.GAMEPLAY)
            {
                LevelSceneName = "Level_1_1", // level loading example
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
            };
            var hubExitSignal = exitSignal.Select(_ => new HubExitContext(gameplayEnterContext)); // send to UI
            var hubViewModel = new HubViewModel(exitSignal);

            hubView.Bind(hubViewModel);
            uiRootBinder.SetViews(hubView);
            
            return hubExitSignal;
        }
    }
}
