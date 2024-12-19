using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;
using LostKaiju.Configs;
using LostKaiju.Architecture.Services;
using LostKaiju.UI.MVVM;
using LostKaiju.UI.MVVM.Hub;

namespace LostKaiju.Architecture.Entry
{
    public class HubBootstrap : MonoBehaviour
    {
        [SerializeField] private CreatureBinderSO _playerBinderSO; // choose your player character
        [SerializeField] private HubView _hubViewPrefab;
        
        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var uiRootBinder = ServiceLocator.Current.Get<UIRootBinder>();

            uiRootBinder.SetView(hubView);

            var exitSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext(Scenes.GAMEPLAY)
            {
                LevelSceneName = "Level_1_1", // level loading example
                PlayerConfig = _playerBinderSO
            };
            var hubExitSignal = exitSignal.Select(_ => new HubExitContext(gameplayEnterContext)); // send to UI
            var hubViewModel = new HubViewModel(exitSignal);

            hubView.Bind(hubViewModel);

            return hubExitSignal;
        }
    }
}
