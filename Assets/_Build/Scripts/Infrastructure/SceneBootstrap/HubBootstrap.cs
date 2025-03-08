using System;
using System.Linq;
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
        private CampaignModel _campaignModel = null;

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var uiRootBinder = Container.Resolve<IRootUIBinder>();

            var exitToGameplaySignal = new Subject<Unit>();
            var exitToMainMenuSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext()
            {
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
            };
            var mainMenuEnterContext = new MainMenuEnterContext();
            var hubExitToMainMenuContext = new HubExitContext(mainMenuEnterContext);
            var hubExitToGameplayContext = new HubExitContext(gameplayEnterContext);
            var hubExitSignal = new Subject<HubExitContext>();

            var gameStateProvider = Container.Resolve<IGameStateProvider>();
            var campaignModelFactory = new CampaignModelFactory();
            campaignModelFactory.OnProduced.Subscribe(x => _campaignModel = x);
            var getCampaignModel = new Func<Task<CampaignModel>>(async () => 
                await campaignModelFactory.GetModelAsync(gameStateProvider));
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var hubViewModel = new HubViewModel(exitToGameplaySignal, getCampaignModel, settingsBinder, uiRootBinder);

            hubView.Bind(hubViewModel);
            uiRootBinder.SetView(hubView);

            exitToGameplaySignal.Take(1).Subscribe(_ =>
            {
                gameplayEnterContext.LevelSceneName = _campaignModel.SelectedMission.Value.SceneName;
                var selectedLocationData = _campaignModel.SelectedLocation.Value;
                var selectedMissionData = _campaignModel.SelectedMission.Value;
                gameplayEnterContext.SelectedLocationData = selectedLocationData;
                gameplayEnterContext.SelectedMissionData = selectedMissionData;
                var missionCompleteSignal = _campaignModel.CreateMissionCompletionSignal(selectedLocationData,
                    selectedMissionData, () => gameStateProvider.SaveCampaignAsync());
                gameplayEnterContext.MissionCompletionSignal = new Subject<Unit>();
                gameplayEnterContext.MissionCompletionSignal.Take(1).Subscribe(_ => 
                {
                    Debug.Log("Campaign - state update");
                    missionCompleteSignal.OnNext(Unit.Default);
                });
                hubExitSignal.OnNext(hubExitToGameplayContext);
            });

            exitToMainMenuSignal.Take(1).Subscribe(_ =>
            {
                hubExitSignal.OnNext(hubExitToMainMenuContext);
            });

            return hubExitSignal;
        }
    }
}
