using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using R3;

using LostKaiju.Infrastructure.Scopes;
using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.UI.MVVM.Gameplay.PlayerCreature;
using LostKaiju.Game.World.Player;
using LostKaiju.Game.World.Player.Behaviour;
using LostKaiju.Game.World.Player.Data.Configs;
using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.World.Missions.Triggers;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.Constants;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Services.Inputs;
using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.UI.MVVM.Shared.Settings;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class CommonMissionBootstrap : MissionBootstrap
    {
        [SerializeField] private Transform _playerInitPosition; 
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private Volume _volume;
        [SerializeField] private PlayerHeroTrigger _missionExitAreaTrigger;
        [SerializeField] private SubSceneTrigger[] _subSceneTriggers;

        public override R3.Observable<MissionExitContext> Boot(MissionEnterContext missionEnterContext)
        {
            var gameplayEnterContext = missionEnterContext.GameplayEnterContext;
            var playerConfigPath = gameplayEnterContext.PlayerConfigPath;
            var playerConfigSO = Resources.Load<PlayerConfigSO>(playerConfigPath);
            var playerPrefab = playerConfigSO.CreatureBinder;
            var spawnPosition = missionEnterContext.FromMissionSceneName == null ? // if from subscene to main
                missionEnterContext.PlayerPosition ?? _playerInitPosition.position : _playerInitPosition.position;
            var player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("player instantiated");

            var playerData = playerConfigSO.PlayerData;
            var healthState = new HealthState(playerData.PlayerDefenceData.MaxHealth);
            var healthModel = new HealthModel(healthState);

            var playerIndicatorsViewModel = new PlayerIndicatorsViewModel(healthModel);
            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var playerIndicatorsViewPrefab = Resources.Load<PlayerIndicatorsView>(Paths.PLAYER_INDICATORS_VIEW);
            var playerIndicatorsView = Instantiate(playerIndicatorsViewPrefab);
            playerIndicatorsView.Bind(playerIndicatorsViewModel);
            rootUIBinder.AddView(playerIndicatorsView);

            var inputProvider = Container.Resolve<IInputProvider>();
            var inputStateProvider = Container.Resolve<InputStateProvider>();
            inputStateProvider.ClearBlockers();
            var playerInputPresenter = new PlayerInputPresenter(playerData.PlayerControlsData, inputProvider);
            inputStateProvider.IsInputEnabled.Subscribe(playerInputPresenter.SetInputEnabled);
            var playerDefencePresenter = new PlayerDefencePresenter(healthModel, playerData.PlayerDefenceData);
            var playerRootPresenter = new PlayerRootPresenter(playerInputPresenter, playerDefencePresenter);
            playerRootPresenter.Bind(player);
            Debug.Log("player root presenter binded");

            if (player.Features.TryResolve<ICameraTarget>(out var cameraTarget) && cameraTarget.TargetTransform != null)
                _cinemachineCamera.Follow = cameraTarget.TargetTransform;
            else
                _cinemachineCamera.Follow = player.transform;

            var exitSignal = new Subject<Unit>();
            
            exitSignal.Take(1).Subscribe(_ =>
            {
                rootUIBinder.ClearView(playerIndicatorsView);
            });
            var toMissionEnterContext = new MissionEnterContext(gameplayEnterContext); // add toMissionSceneName
            var missionExitContext = new MissionExitContext(toMissionEnterContext);
            var missionExitSignal = exitSignal.Select(_ => missionExitContext); // for transition between one mission scenes
            var gameplayExitSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;
            
#region Death handling
            healthModel.CurrentHealth.Where(x => x == 0).TakeUntil(exitSignal).Subscribe(_ => 
            {
                OnPlayerTransition();
                player.gameObject.SetActive(false);
                Observable.Timer(TimeSpan.FromSeconds(1)).TakeUntil(exitSignal).Subscribe(_ =>
                {
                    // player.transform.position = _playerInitPosition.position;
                    // healthModel.RestoreFullHealth();
                    toMissionEnterContext.PlayerPosition = null;
                    toMissionEnterContext.FromMissionSceneName = null;
                    toMissionEnterContext.FromTriggerId = null;
                    missionExitContext.ToMissionSceneName = gameplayEnterContext.LevelSceneName;

                    exitSignal.OnNext(Unit.Default);
                });
            });
#endregion

            // if (isMultuplayer)
            // {
            //     var groupTrigger = new GroupTriggerObserver<IPlayerHero>(_missionExitAreaTrigger);
            // }
#region Subscene trigger handling
            if (_subSceneTriggers != null)
            {
                var shouldSkip = !String.IsNullOrEmpty(missionEnterContext.FromTriggerId);
                foreach (var subTrigger in _subSceneTriggers)
                {
                    if (subTrigger == null) 
                    {
                        Debug.LogWarning("SubScene trigger is null");
                        continue;
                    }
                    // TODO: not to spawn hero in trigger or add button or another option to approve transitions on trigger stay
                    var onTriggerEnter = subTrigger.OnEnter;
                    if (shouldSkip && subTrigger.ToSceneName == missionEnterContext.FromTriggerId)
                    {
                        onTriggerEnter = onTriggerEnter.Skip(1);
                    }
                    
                    onTriggerEnter
                        .Take(1)
                        .Subscribe(_ =>
                        {
                            Debug.Log("SubScene signal");
                            Debug.Log((subTrigger.transform.position - player.transform.position).magnitude);
                            toMissionEnterContext.FromMissionSceneName = missionEnterContext.GameplayEnterContext.LevelSceneName;
                            toMissionEnterContext.PlayerPosition = player.transform.position;
                            toMissionEnterContext.FromTriggerId = subTrigger.ToSceneName;
                            missionExitContext.ToMissionSceneName = subTrigger.ToSceneName;
                            OnPlayerTransition();
                            exitSignal.OnNext(Unit.Default);
                        });
                }
            }
#endregion

#region Mission complete area
            if (_missionExitAreaTrigger != null)
            {
                if (!String.IsNullOrEmpty(missionEnterContext.FromMissionSceneName)) // from main mission scene to subscene
                {
                    _missionExitAreaTrigger.OnEnter.Take(1).Subscribe(_ =>
                    {
                        Debug.Log("Exit SubScene signal");
                        toMissionEnterContext.PlayerPosition = missionEnterContext.PlayerPosition;
                        toMissionEnterContext.FromTriggerId = missionEnterContext.FromTriggerId;
                        missionExitContext.ToMissionSceneName = missionEnterContext.FromMissionSceneName;
                        OnPlayerTransition();
                        exitSignal.OnNext(Unit.Default);
                    });
                }
                else
                {
                    _missionExitAreaTrigger.OnEnter.Take(1).Subscribe(_ => // current mission scene is main
                    {
                        Debug.Log("Mission completed signal");
                        missionEnterContext.GameplayEnterContext.MissionCompletionSignal.OnNext(Unit.Default);
                        OnPlayerTransition();
                        gameplayExitSignal.OnNext(Unit.Default);
                    });
                }
            }
            else
            {
                Debug.LogError("Mission Exit Area Trigger is null");
            }
#endregion
            
            var settingsModel = Container.Resolve<SettingsModel>();
            BindPostProcessing(settingsModel);

            return missionExitSignal;

            void OnPlayerTransition()
            {
                inputStateProvider.AddBlocker(new FakeBlocker());
                if (Container.TryResolve<OptionsBinder>(out var optionsBinder))
                    optionsBinder.CloseAll();
                playerDefencePresenter.SetInvincible(true);
            }
        }

        private void BindPostProcessing(SettingsModel settingsModel)
        {
            if (_volume == null)
            {
                Debug.LogWarning("No Volume found");
            }
            else
            {
                settingsModel.IsPostProcessingEnabled.Subscribe(x => _volume.enabled = x);

                if (_volume.profile.TryGet<Bloom>(out var bloom))
                    settingsModel.IsHighBloomQuality.Subscribe(x => bloom.highQualityFiltering.value = x);
                else
                    Debug.LogWarning("No Bloom in VolumeProfile found");   
            }
        }
    }
}
