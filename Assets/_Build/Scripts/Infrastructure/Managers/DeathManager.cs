using System;
using R3;

using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.World.Player.Data.Configs;
using LostKaiju.Services.Inputs;
using UnityEngine;

namespace LostKaiju.Infrastructure.Managers
{
    public class DeathManager : IDisposable
    {
        private readonly PlayerManager _playerManager;
        private readonly CameraManager _cameraManager;
        private readonly Transform _spawnTransform;
        private readonly CompositeDisposable _disposables = new();

        public DeathManager(
            PlayerManager playerManager,
            CameraManager cameraManager,
            Transform spawnTransform)
        {
            _playerManager = playerManager;
            _cameraManager = cameraManager;
            _spawnTransform = spawnTransform;
        }

        public void Initialize()
        {
            _playerManager.HealthModel.IsDead
                .Where(x => x)
                .Subscribe(_ => HandleDeath())
                .AddTo(_disposables);
        }

        private void HandleDeath()
        {
            _cameraManager.StopFollowing();
            _playerManager.DisposePlayer();

            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ => RespawnPlayer())
                .AddTo(_disposables);
        }

        private void RespawnPlayer()
        {
            _playerManager.RespawnPlayer(_spawnTransform.position);
            _cameraManager.FollowCreature(_playerManager.PlayerCreature);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}