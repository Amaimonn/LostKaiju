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
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Game.Constants;
using LostKaiju.Services.Audio;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : LifetimeScope
    {
        [SerializeField] private HubView _hubViewPrefab;
        private CampaignModel _campaignModel = null;

        public async Task<Observable<HubExitContext>> BootAsync(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var gameStateProvider = Container.Resolve<IGameStateProvider>();
            if (gameStateProvider.Heroes == null)
                await gameStateProvider.LoadHeroesAsync();

            var hubExitSignal = new Subject<HubExitContext>();
            var exitToGameplaySignal = new Subject<Unit>();
            var exitToMainMenuSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext();
            var mainMenuEnterContext = new MainMenuEnterContext();
            var hubExitToMainMenuContext = new HubExitContext(mainMenuEnterContext);
            var hubExitToGameplayContext = new HubExitContext(gameplayEnterContext);

            var campaignModelFactory = new CampaignModelFactory(gameStateProvider);
            campaignModelFactory.OnProduced.Subscribe(x => _campaignModel = x);
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var inputProvider = Container.Resolve<IInputProvider>();
            settingsBinder.BindClosingSignal(inputProvider.OnEscape.TakeUntil(hubExitSignal));
            var heroesModelFactory = new HeroesModelFactory(gameStateProvider);
            heroesModelFactory.OnProduced.Subscribe(x => x.SelectedHeroData.Skip(1).Subscribe(_ => gameStateProvider.SaveHeroesAsync()));
            var heroSelectionBinder = new HeroSelectionBinder(rootUIBinder, heroesModelFactory);
            var audioPlayer = Container.Resolve<AudioPlayer>();
            var hubViewModel = new HubViewModel(exitToGameplaySignal, campaignModelFactory, settingsBinder, 
                heroSelectionBinder, rootUIBinder, audioPlayer);

            hubView.Bind(hubViewModel);
            rootUIBinder.SetView(hubView);

            exitToGameplaySignal.Take(1).Subscribe(_ =>
            {
                gameplayEnterContext.PlayerConfigPath = $"{Paths.PLAYER_CREATURES}/{gameStateProvider.Heroes.SelectedHeroId}";
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
