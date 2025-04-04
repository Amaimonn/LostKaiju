using UnityEngine;
using R3;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;
using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Player.Views
{
    public class PlayerBinder : CreatureBinder, ICreatureUpdater, IPlayerHero, ICameraTarget
    {    
#region ICameraTarget
        public Transform TargetTransform => _cameraTargetTransform;
#endregion

        [Header("Creature features")]
        [SerializeField] private Transform _cameraTargetTransform;
        [SerializeField] private Flipper _flipper;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private DamageReceiver _damageReceiver;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private Pusher _pusher;
        [SerializeField] private PlayerJuicySystem _playerJuicySystem;

        protected IUpdatablePresenter _updatablePresenter;

        #region ICreatureUpdater
        public void SetUpdatablePresenter(IUpdatablePresenter updatablePresenter)
        {
            _updatablePresenter = updatablePresenter;
        }
#endregion

        private void Awake()
        {
            _features.Register<ICameraTarget>(this);
            _features.Register<IFlipper>(_flipper);
            _features.Register<IGroundCheck>(_groundCheck);
            _features.Register<IDamageReceiver>(_damageReceiver);
            _features.Register<ICreatureUpdater>(this);
            _features.Register<IPusher>(_pusher);
            _features.Register<IAttacker>(_attacker);
            _features.Register<PlayerJuicySystem>(_playerJuicySystem);
            Debug.Log("Player features registered");
        }

        private void Update()
        {
            _updatablePresenter?.UpdateLogic();
        }

        private void FixedUpdate()
        {
            _updatablePresenter?.FixedUpdateLogic();
        }

        private void OnDestroy()
        {
            _onDispose.OnNext(Unit.Default);
        }
    }
}
