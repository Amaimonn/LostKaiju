using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering;
using VContainer;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.World.Missions.Triggers;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Services.Inputs;
using LostKaiju.Game.World.Enemy;
using LostKaiju.Infrastructure.Managers;
using LostKaiju.Infrastructure.Scopes;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class CommonMissionBootstrap : MissionBootstrap
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private Volume _volume;
        [SerializeField] private PlayerHeroTrigger _missionExitAreaTrigger;
        [SerializeField] private SubSceneTrigger[] _subSceneTriggers;
        [SerializeField] private EnemyInjector _enemyInjector;
        
        // private CompositeDisposable _disposables = new();
        private PlayerManager _playerManager;
        private CameraManager _cameraManager;
        private SceneTransitionManager _sceneTransitionManager;
        private PostProcessingManager _postProcessingManager;
        private DeathManager _deathManager;

        public override R3.Observable<MissionExitContext> Boot(MissionEnterContext missionEnterContext)
        {
            var gameplayEnterContext = missionEnterContext.GameplayEnterContext;
            var spawnPosition = GetSpawnPosition(missionEnterContext);
            var gameplayExitSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;

            InitializeManagers(gameplayEnterContext);
            _playerManager.FirstSpawnPlayer(spawnPosition);
            _cameraManager.FollowCreature(_playerManager.PlayerCreature);

            var toMissionEnterContext = new MissionEnterContext(gameplayEnterContext);
            var missionExitContext = new MissionExitContext(toMissionEnterContext);

            _sceneTransitionManager.Initialize(missionEnterContext, missionExitContext);
            _deathManager.Initialize();

            _enemyInjector.InjectAndInitAll(Container);
            
            _sceneTransitionManager.ExitSignal.Merge(gameplayExitSignal).Subscribe(_ => 
            {
                _postProcessingManager.Dispose();
                _deathManager.Dispose();
                _playerManager.Dispose();
                _sceneTransitionManager.Dispose();
            });

            return _sceneTransitionManager.ExitSignal
                .Select(_ => missionExitContext);
        }

        private void InitializeManagers(GameplayEnterContext gameplayEnterContext)
        {
            _playerManager = new PlayerManager(
                Container,
                gameplayEnterContext.PlayerConfig,
                Container.Resolve<IInputProvider>(),
                Container.Resolve<InputStateProvider>(),
                Container.Resolve<IRootUIBinder>());

            _cameraManager = new CameraManager(_cinemachineCamera);

            _sceneTransitionManager = new SceneTransitionManager(
                _playerManager,
                _subSceneTriggers,
                _missionExitAreaTrigger);

            _playerManager.Init(_sceneTransitionManager.ExitSignal);

            _postProcessingManager = new PostProcessingManager(_volume);
            _postProcessingManager.BindFromSettings(Container.Resolve<SettingsModel>());

            _deathManager = new DeathManager(
                _playerManager,
                _cameraManager,
                _playerInitPosition);
        }

        private Vector3 GetSpawnPosition(MissionEnterContext missionEnterContext)
        {
            return missionEnterContext.FromMissionSceneName == null
                ? missionEnterContext.PlayerPosition ?? _playerInitPosition.position
                : _playerInitPosition.position;
        }
    }
}
