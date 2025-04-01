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
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TypedRegistration<MissionExitContext, Subject<Unit>>>(Lifetime.Singleton);

            builder.RegisterInstance<CameraManager>(new CameraManager(_cinemachineCamera));

            builder.RegisterInstance<PostProcessingManager>(new PostProcessingManager(_volume));

            builder.Register<SceneTransitionManager>(resolver => 
            {
                return new SceneTransitionManager(
                    resolver.Resolve<PlayerManager>(),
                    _subSceneTriggers,
                    _missionExitAreaTrigger,
                    resolver.Resolve<TypedRegistration<MissionExitContext, Subject<Unit>>>().Instance);
            }, Lifetime.Singleton);

            builder.Register<PlayerManager>(resolver =>
            {
                return new PlayerManager(
                    Container,
                    _missionEnterContext.GameplayEnterContext.PlayerConfig,
                    resolver.Resolve<IInputProvider>(),
                    resolver.Resolve<InputStateProvider>(),
                    resolver.Resolve<IRootUIBinder>(),
                    resolver.Resolve<TypedRegistration<MissionExitContext, Subject<Unit>>>().Instance);
            }, Lifetime.Singleton);

            builder.Register<DeathManager>(resolver =>
            {
                return new DeathManager(
                    resolver.Resolve<PlayerManager>(),
                    resolver.Resolve<CameraManager>(),
                    _playerInitPosition);
            }, Lifetime.Singleton);
        }

        public override R3.Observable<MissionExitContext> Boot(MissionEnterContext missionEnterContext)
        {
            _missionEnterContext = missionEnterContext;
            Build();

            var missionExitSignal = Container.Resolve<TypedRegistration<MissionExitContext, Subject<Unit>>>().Instance;
            var gameplayEnterContext = missionEnterContext.GameplayEnterContext;
            var spawnPosition = GetSpawnPosition(missionEnterContext);
            var gameplayExitSignal = Container.Resolve<TypedRegistration<GameplayExitContext, Subject<Unit>>>().Instance;
            var toMissionEnterContext = new MissionEnterContext(gameplayEnterContext);
            var missionExitContext = new MissionExitContext(toMissionEnterContext);

            var postProcessingManager = Container.Resolve<PostProcessingManager>();
            postProcessingManager.BindFromSettings(Container.Resolve<SettingsModel>());

            var playerManager  = Container.Resolve<PlayerManager>();
            playerManager.FirstSpawnPlayer(spawnPosition);

            var cameraManager = Container.Resolve<CameraManager>();
            cameraManager.FollowCreature(playerManager.PlayerCreature);
            
            var sceneTransitionManager = Container.Resolve<SceneTransitionManager>();
            sceneTransitionManager.Init(missionEnterContext, missionExitContext);

            var deathManager = Container.Resolve<DeathManager>();
            deathManager.Init();

            _enemyInjector.InjectAndInitAll(Container);
            
            missionExitSignal.Merge(gameplayExitSignal).Subscribe(_ => 
            {
                postProcessingManager.Dispose();
                deathManager.Dispose();
                playerManager.Dispose();
                sceneTransitionManager.Dispose();
            });

            return missionExitSignal.Select(_ => missionExitContext);
        }

        private Vector3 GetSpawnPosition(MissionEnterContext missionEnterContext)
        {
            return missionEnterContext.FromMissionSceneName == null
                ? missionEnterContext.PlayerPosition ?? _playerInitPosition.position
                : _playerInitPosition.position;
        }
    }
}
