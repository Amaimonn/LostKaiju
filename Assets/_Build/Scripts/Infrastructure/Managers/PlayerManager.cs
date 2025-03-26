using System;
using UnityEngine;
using R3;
using VContainer;
using Object = UnityEngine.Object;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Gameplay.PlayerCreature;
using LostKaiju.Game.Constants;
using LostKaiju.Game.GameData.HealthSystem;
using LostKaiju.Game.Providers.InputState;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Player;
using LostKaiju.Game.World.Player.Behaviour;
using LostKaiju.Game.World.Player.Data.Configs;
using LostKaiju.Game.World.Player.Views;
using LostKaiju.Services.Inputs;

namespace LostKaiju.Infrastructure.Managers
{
    public class PlayerManager : IDisposable
    {
        public CreatureBinder PlayerCreature => _playerCreature;
        public Transform PlayerTransform => _playerCreature.transform;
        public HealthModel HealthModel => _healthModel;
        
        private readonly IObjectResolver _container;
        private readonly IPlayerConfig _playerConfig;
        private readonly IInputProvider _inputProvider;
        private readonly InputStateProvider _inputStateProvider;
        private readonly IRootUIBinder _rootUIBinder;

        private CreatureBinder _playerCreature;
        private PlayerRootPresenter _playerRootPresenter;
        private IPlayerInputPresenter _playerInputPresenter;
        private IPlayerDefencePresenter _playerDefencePresenter;
        private HealthModel _healthModel;
        private PlayerIndicatorsView _playerIndicatorsView;
        private CompositeDisposable _playerDisposables = new();

        public PlayerManager(
            IObjectResolver container,
            IPlayerConfig playerConfig,
            IInputProvider inputProvider,
            InputStateProvider inputStateProvider,
            IRootUIBinder rootUIBinder)
        {
            _container = container;
            _playerConfig = playerConfig;
            _inputProvider = inputProvider;
            _inputStateProvider = inputStateProvider;
            _rootUIBinder = rootUIBinder;
        }

        public void Init(Observable<Unit> clearViewsSignal)
        {
            clearViewsSignal.Take(1).Subscribe(_ => ClearViews());
        }

        public void SpawnPlayer(Vector3 position)
        {
            InitPlayerCreature(position);
            InitHealthSystem();
            InitInput();
            InitUI();
        }

        public void RespawnPlayer(Vector3 position)
        {
            _healthModel.Revive();
            InitPlayerCreature(position);
            InitInput();
        }

        private void InitPlayerCreature(Vector3 position)
        {
            _playerCreature = Object.Instantiate(_playerConfig.CreatureBinder, position, Quaternion.identity);

            if (_playerCreature.Features.TryResolve<PlayerJuicySystem>(out var juicySystem))
                _container.Inject(juicySystem);
        }

        private void InitHealthSystem()
        {
            var healthState = new HealthState(_playerConfig.PlayerData.PlayerDefenceData.MaxHealth);
            _healthModel = new HealthModel(healthState);
        }

        private void InitInput()
        {
            _playerInputPresenter = new PlayerInputPresenter(
                _playerConfig.PlayerData.PlayerControlsData,
                _inputProvider);

            _inputStateProvider.IsInputEnabled.Subscribe(_playerInputPresenter.SetInputEnabled)
                .AddTo(_playerDisposables);

            _playerDefencePresenter = new PlayerDefencePresenter(
                _healthModel,
                _playerConfig.PlayerData.PlayerDefenceData);

            _playerRootPresenter = new PlayerRootPresenter(
                _playerInputPresenter,
                _playerDefencePresenter);

            _playerRootPresenter.Bind(_playerCreature);
        }

        private void InitUI()
        {
            var playerIndicatorsViewModel = new PlayerIndicatorsViewModel(_healthModel);
            var playerIndicatorsViewPrefab = Resources.Load<PlayerIndicatorsView>(Paths.PLAYER_INDICATORS_VIEW);
            _playerIndicatorsView = Object.Instantiate(playerIndicatorsViewPrefab);
            _playerIndicatorsView.Bind(playerIndicatorsViewModel);
            _rootUIBinder.AddView(_playerIndicatorsView);
        }

        public void SetInvincible(bool isInvincible)
        {
            _playerDefencePresenter.SetInvincible(isInvincible);
        }

        private void ClearViews()
        {
            _rootUIBinder.ClearView(_playerIndicatorsView);
        }

        public void DisposePlayer()
        {
            _playerRootPresenter?.Dispose();
            _playerDisposables.Dispose();
            _playerDisposables = new();

            if (_playerCreature != null)
                Object.Destroy(_playerCreature.gameObject);
        }

        public void Dispose()
        {
            _playerDisposables.Dispose();
        }
    }
}