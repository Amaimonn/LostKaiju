using System;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.Scopes;
using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Hub;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.UI.CustomElements;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Game.Constants;
using LostKaiju.Game.GameData;
using LostKaiju.Services.Inputs;
using LostKaiju.Services.Audio;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : LifetimeScope
    {
        // [SerializeField] private HubView _hubViewPrefab;
        [SerializeField] private HubCanvasView _hubViewPrefab;
        [SerializeField] private CameraRenderTextureSetter _heroRenderTextureSetter;
        [SerializeField] private Transform _heroPreviewTransform;
        [SerializeField] private AudioClip _hubMusic;
        private CampaignModel _campaignModel = null;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CampaignModelFactory>(Lifetime.Singleton).As<ILoadableModelFactory<CampaignModel>>();
            builder.Register<TypedRegistration<GameplayEnterContext, Subject<Unit>>>(Lifetime.Singleton);
            builder.Register<TypedRegistration<MainMenuEnterContext, Subject<Unit>>>(Lifetime.Singleton);
            builder.Register<CampaignNavigationBinder>(resolver => 
            {
                var rootUIBinder = Container.Resolve<IRootUIBinder>();
                var factory = Container.Resolve<ILoadableModelFactory<CampaignModel>>();
                var audioPlayer = Container.Resolve<AudioPlayer>();
                var exitToGameplaySignal = Container.Resolve<TypedRegistration<GameplayEnterContext, Subject<Unit>>>().Instance;
                var binder = new CampaignNavigationBinder(rootUIBinder, factory, audioPlayer, exitToGameplaySignal);
                return binder;
            }, Lifetime.Singleton);
        }

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext = null)
        {
            var hubExitSignal = new Subject<HubExitContext>();

            var audioPlayer = Container.Resolve<AudioPlayer>();
            audioPlayer.PlayMusic(_hubMusic, true);

            var exitToGameplaySignal = Container.Resolve<TypedRegistration<GameplayEnterContext, Subject<Unit>>>().Instance;
            var exitToMainMenuSignal = Container.Resolve<TypedRegistration<MainMenuEnterContext, Subject<Unit>>>().Instance;
            var gameplayEnterContext = new GameplayEnterContext();
            var mainMenuEnterContext = new MainMenuEnterContext();
            var hubExitToMainMenuContext = new HubExitContext(mainMenuEnterContext);
            var hubExitToGameplayContext = new HubExitContext(gameplayEnterContext);

            var hubView = Instantiate(_hubViewPrefab);
            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var gameStateProvider = Container.Resolve<IGameStateProvider>();

            var campaignModelFactory = Container.Resolve<ILoadableModelFactory<CampaignModel>>();
            campaignModelFactory.OnProduced.Subscribe(x => _campaignModel = x);

            var campaignBinder = Container.Resolve<CampaignNavigationBinder>();
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var inputProvider = Container.Resolve<IInputProvider>();
            settingsBinder.BindClosingSignal(inputProvider.OnEscape.TakeUntil(hubExitSignal));
            var heroesModelFactory = new HeroesModelFactory(gameStateProvider);
            heroesModelFactory.OnProduced.Subscribe(x => x.SelectedHeroData.Skip(1).Subscribe(_ => gameStateProvider.SaveHeroesAsync()));
            _heroRenderTextureSetter.Init();
            var heroSelectionBinder = new HeroSelectionBinder(rootUIBinder, heroesModelFactory, 
                _heroRenderTextureSetter.CurrentRenderTexture);
            var hubViewModel = new HubViewModel(campaignBinder, settingsBinder, heroSelectionBinder);

            var heroPreview = new HeroPreview(_heroPreviewTransform, onShowPreview: x => x.SetActive(true), 
                onHidePreview: x => x.SetActive(false));
            
            var heroPreviewSelector = new HeroPreviewSelector();
            heroPreview.SetPreview(heroPreviewSelector.GetPreviewById(gameStateProvider.Heroes.SelectedHeroId));
            
            heroSelectionBinder.OnOpened.Subscribe(vm => 
            {
                vm.CurrentHeroDataPreview.Where(value => value != null)
                    .Skip(1)
                    .Subscribe(currentHeroData =>
                    {
                        heroPreview.SetPreview(heroPreviewSelector.GetPreviewById(currentHeroData.Id));
                    });
                    
                vm.OnClosingCompleted.Take(1).Subscribe(_ => 
                {
                    heroPreviewSelector.ClearExceptOne(gameStateProvider.Heroes.SelectedHeroId);
                });
            });

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

            if (hubEnterContext != null) // passed context
            {
                if (!String.IsNullOrEmpty(hubEnterContext.ExitingMissionId))
                {
                    if (hubEnterContext.IsMissionCompleted)
                        Debug.Log($"<color=#008000>Mission {hubEnterContext.ExitingMissionId} completed</color>");
                    else
                        Debug.Log($"<color=#FFFF00>Mission {hubEnterContext.ExitingMissionId} exited</color>");
                    hubViewModel.OpenCampaign();
                }
            }
            
            return hubExitSignal;
        }
    }
}
