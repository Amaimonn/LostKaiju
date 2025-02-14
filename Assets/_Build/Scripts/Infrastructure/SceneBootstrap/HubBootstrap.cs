using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Hub;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign.Locations;


namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : LifetimeScope
    {
        [SerializeField] private HubView _hubViewPrefab;
        [SerializeField] private string _playerConfigName; // choose your player character
        [SerializeField] private string _locationsConfigPath;

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var uiRootBinder = Container.Resolve<IRootUIBinder>();

            var exitSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext()
            {
                LevelSceneName = "Mission_1_1", // mission loading example
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
            };
            var hubExitContext = new HubExitContext(gameplayEnterContext);
            var hubExitSignal = exitSignal.Select(_ => hubExitContext); // send to UI

            var gameStateProvider = Container.Resolve<IGameStateProvider>();

            var campaignModelFactory = new Func<CampaignModel>(() => CampaignModelFactory(gameplayEnterContext, 
                gameStateProvider));
            var hubViewModel = new HubViewModel(exitSignal, campaignModelFactory, uiRootBinder);

            hubView.Bind(hubViewModel);
            uiRootBinder.SetView(hubView);

            return hubExitSignal;
        }

        private CampaignModel CampaignModelFactory(GameplayEnterContext gameplayEnterContext, IGameStateProvider gameStateProvider)
        {
            var locationsDataSO = Resources.Load<LocationsDataSO>(_locationsConfigPath);
            var locationsPairs = locationsDataSO.Locations
                .Select(x => new KeyValuePair<string, ILocationData>(x.Id, x));
            var locationsMap = new Dictionary<string, ILocationData>(locationsPairs);
            var campaignState = gameStateProvider.Campaign;
            var campaignModel = new CampaignModel(campaignState, locationsMap);

            campaignModel.SelectedMission.Subscribe(x => {
                if (x != null)
                {
                    gameplayEnterContext.LevelSceneName = x.SceneName;
                    Debug.Log($"r3: Scene in signal: {x.SceneName}");
                }
            });

            return campaignModel;
        }
    }
}
