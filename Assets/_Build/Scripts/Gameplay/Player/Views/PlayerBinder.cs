using UnityEngine;

using LostKaiju.Utils;
using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Creatures.Presenters;

namespace LostKaiju.Game.Player.Views
{
    public class PlayerBinder : CreatureBinder, ICreatureUpdater
    {    
#region CreatureBinder
        public override Rigidbody2D Rigidbody => _rigidbody;
        public override Animator Animator => _animator;
        public override Holder<ICreatureFeature> Features => _features;
#endregion

        [Header("Creature base")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;

        [Header("Creature features")]
        [SerializeField] private Flipper _flipper;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private DamageReceiver _damageReceiver;
        [SerializeField] private Attacker _attacker;

        protected CreaturePresenter _currentPresenter;
        protected Holder<ICreatureFeature> _features = new();

#region ICreatureUpdater
        public void SetCreaturePresenter(CreaturePresenter creaturePresenter)
        {
            _currentPresenter = creaturePresenter;
        }
#endregion

        private void Awake()
        {
            _features.Register<Flipper>(_flipper);
            _features.Register<GroundCheck>(_groundCheck);
            _features.Register<DamageReceiver>(_damageReceiver);
            _features.Register<ICreatureUpdater>(this);
            _features.Register<IAttacker>(_attacker);
            Debug.Log("Player features registered");
        }

        private void Update()
        {
            _currentPresenter?.UpdateLogic();
        }

        private void FixedUpdate()
        {
            _currentPresenter?.FixedUpdateLogic();
        }
    }
}
