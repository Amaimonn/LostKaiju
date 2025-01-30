using System;
using System.Linq;
using UnityEngine;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.Locator;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Hub;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign.Locations;
using System.Collections.Generic;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : MonoBehaviour
    {
        [SerializeField] private HubView _hubViewPrefab;
        [SerializeField] private string _playerConfigName; // choose your player character
        [SerializeField] private string _locationsConfigPath;
        private IGameStateProvider _gameStateProvider;

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var serviceLocator = ServiceLocator.Instance;
            var uiRootBinder = serviceLocator.Get<IRootUIBinder>();

            var exitSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext(Scenes.GAMEPLAY)
            {
                LevelSceneName = "Mission_1_1", // mission loading example
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
            };
            var hubExitContext = new HubExitContext(gameplayEnterContext);
            var hubExitSignal = exitSignal.Select(_ => hubExitContext); // send to UI

            _gameStateProvider = serviceLocator.Get<IGameStateProvider>();


            var campaignModelFactory = new Func<CampaignModel>(() => CampaignModelFactory(gameplayEnterContext));
            var hubViewModel = new HubViewModel(exitSignal, campaignModelFactory);

            hubView.Bind(hubViewModel);
            uiRootBinder.SetView(hubView);

            return hubExitSignal;
        }

        private CampaignModel CampaignModelFactory(GameplayEnterContext gameplayEnterContext)
        {
            var locationsDataSO = Resources.Load<LocationsDataSO>(_locationsConfigPath);
            var locationsPairs = locationsDataSO.Locations
                .Select(x => new KeyValuePair<string, ILocationData>(x.Id, x));
            var locationsMap = new Dictionary<string, ILocationData>(locationsPairs);
            var selectedLocation = locationsMap.First().Value; // test location selection
            var selectedMission = selectedLocation.AllMissionsData.FirstOrDefault(); // test mission selection
            var campaignState = _gameStateProvider.Campaign;
            var campaignModel = new CampaignModel(campaignState, locationsMap, selectedMission);

            campaignModel.SelectedMission.Subscribe(x => {
                gameplayEnterContext.LevelSceneName = x.SceneName;
                Debug.Log($"r3: Scene in signal: {x.SceneName}");
            });

            return campaignModel;
        }
    }
}
