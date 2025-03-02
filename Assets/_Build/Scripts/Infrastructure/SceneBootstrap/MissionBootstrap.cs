using UnityEngine;
using Unity.Cinemachine;
using VContainer.Unity;
using VContainer;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.UI.MVVM.Gameplay.PlayerCreature;
using LostKaiju.Game.World.Player;
using LostKaiju.Game.World.Player.Behaviour;
using LostKaiju.Game.World.Player.Data.Configs;
using LostKaiju.Game.World.Player.Data.Indicators;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Services.Inputs;
using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.World.Missions;
using LostKaiju.Infrastructure.Scopes;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MissionBootstrap : LifetimeScope
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private MissionExitAreaTrigger _missionExitAreaTrigger;
        [SerializeField] private string _playerIndicatorsViewPrefabPath = "UI/Gameplay/PlayerIndicatorsView";

        public Observable<MissionExitContext> Boot(MissionEnterContext missionEnterContext)
        {
            var gameplayEnterContext = missionEnterContext.GameplayEnterContext;
            var playerConfigPath = gameplayEnterContext.PlayerConfigPath;
            var playerConfigSO = Resources.Load<PlayerConfigSO>(playerConfigPath);
            var playerPrefab = playerConfigSO.CreatureBinder;
            var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            Debug.Log("player instantiated");

            var playerData = playerConfigSO.PlayerData;
            var healthState = new HealthState(playerData.PlayerDefenceData.MaxHealth);
            var healthModel = new HealthModel(healthState);
            var playerIndicatorsViewModel = new PlayerIndicatorsViewModel(healthModel);
            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var playerIndicatorsViewPrefab = Resources.Load<PlayerIndicatorsView>(_playerIndicatorsViewPrefabPath);
            var playerIndicatorsView = Instantiate(playerIndicatorsViewPrefab);
            playerIndicatorsView.Bind(playerIndicatorsViewModel);
            rootUIBinder.AddView(playerIndicatorsView);

            var inputProvider = Container.Resolve<IInputProvider>();
            var inputStateProvider = Container.Resolve<InputStateProvider>();
            var playerInputPresenter = new PlayerInputPresenter(playerData.PlayerControlsData, inputProvider);
            inputStateProvider.IsInputEnabled.Subscribe(playerInputPresenter.SetInputEnabled);
            var playerDefencePresenter = new PlayerDefencePresenter(healthModel, playerData.PlayerDefenceData);
            var playerRootPresenter = new PlayerRootPresenter(playerInputPresenter, playerDefencePresenter);
            playerRootPresenter.Bind(player);
            Debug.Log("player root presenter binded");

            var exitSignal = new Subject<Unit>();
            exitSignal.Take(1).Subscribe(_ =>
            {
                rootUIBinder.ClearView(playerIndicatorsView);
            });
            var toMissionEnterContext = new MissionEnterContext(gameplayEnterContext); // add toMissionSceneName
            var missionExitContext = new MissionExitContext(missionEnterContext);
            var missionExitSignal = exitSignal.Select(_ => missionExitContext); // for transition between one mission scenes
            var gameplayExitSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;
            _cinemachineCamera.Follow = player.transform;
            if (_missionExitAreaTrigger != null)
            {
                _missionExitAreaTrigger.OnPlayerHeroEnter.Take(1).Subscribe(_ =>
                {
                    Debug.Log("Mission completed signal");
                    missionEnterContext.GameplayEnterContext.MissionCompletionSignal.OnNext(Unit.Default);
                    gameplayExitSignal.OnNext(Unit.Default);
                });
            }
            else
            {
                Debug.LogError("MissionExitAreaTrigger is null");
            }

            return missionExitSignal;
        }
    }
}
