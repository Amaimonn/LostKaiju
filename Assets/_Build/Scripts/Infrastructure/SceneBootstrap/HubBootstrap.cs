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
using LostKaiju.Services.Inputs;

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

            var hubExitSignal = new Subject<HubExitContext>();
            var exitToGameplaySignal = new Subject<Unit>();
            var exitToMainMenuSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext()
            {
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
            };
            var mainMenuEnterContext = new MainMenuEnterContext();
            var hubExitToMainMenuContext = new HubExitContext(mainMenuEnterContext);
            var hubExitToGameplayContext = new HubExitContext(gameplayEnterContext);

            var gameStateProvider = Container.Resolve<IGameStateProvider>();
            var campaignModelFactory = new CampaignModelFactory();
            campaignModelFactory.OnProduced.Subscribe(x => _campaignModel = x);
            var getCampaignModel = new Func<Task<CampaignModel>>(async () => 
                await campaignModelFactory.GetModelAsync(gameStateProvider));
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var inputProvider = Container.Resolve<IInputProvider>();
            settingsBinder.BindClosingSignal(inputProvider.OnEscape.TakeUntil(hubExitSignal));
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

            if (!String.IsNullOrEmpty(hubEnterContext.ExitingMissionId))
            {
                if (hubEnterContext.IsMissionCompleted)
                    Debug.Log($"<color=#008000>Mission {hubEnterContext.ExitingMissionId} completed</color>");
                else
                    Debug.Log($"<color=#FFFF00>Mission {hubEnterContext.ExitingMissionId} exited</color>");
                hubViewModel.OpenCampaign();
            }

            return hubExitSignal;
        }
    }
}
