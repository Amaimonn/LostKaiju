using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Hub;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Campaign.Locations;


namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : LifetimeScope
    {
        [SerializeField] private HubView _hubViewPrefab;
        [SerializeField] private string _playerConfigName; // choose your player character
        [SerializeField] private string _locationsConfigPath;
        private CampaignModel _campaignModel = null;

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var uiRootBinder = Container.Resolve<IRootUIBinder>();

            var exitSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext()
            {
                LevelSceneName = "Mission_1_1", // mission loading example
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
                SelectedMissionModel = null,
                SelectedLocationModel = null
            };
            var hubExitContext = new HubExitContext(gameplayEnterContext);
            var hubExitSignal = exitSignal.Select(_ => hubExitContext); // send to UI
            var gameStateProvider = Container.Resolve<IGameStateProvider>();
            var campaignModelFactory = new Func<Task<CampaignModel>>(async () => await CampaignModelFactory(gameplayEnterContext, 
                gameStateProvider));
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var hubViewModel = new HubViewModel(exitSignal, campaignModelFactory, settingsBinder, uiRootBinder);

            hubView.Bind(hubViewModel);
            uiRootBinder.SetView(hubView);

            return hubExitSignal;
        }

        private async Task<CampaignModel> CampaignModelFactory(GameplayEnterContext gameplayEnterContext, 
            IGameStateProvider gameStateProvider)
        {
            var locationsDataSORequest = Resources.LoadAsync<LocationsDataSO>(_locationsConfigPath);
            await locationsDataSORequest;

            var locationsDataSO = locationsDataSORequest.asset as LocationsDataSO;
            var campaignState = gameStateProvider.Campaign;
            _campaignModel = new CampaignModel(campaignState, locationsDataSO);

            _campaignModel.SelectedMission.Subscribe(x => {
                if (x != null)
                {
                    gameplayEnterContext.LevelSceneName = x.SceneName;
                    // gameplayEnterContext.SelectedMissionModel = x;
                    Debug.Log($"r3: Scene in signal: {x.SceneName}");
                }
            });

            return _campaignModel;
        }
    }
}
