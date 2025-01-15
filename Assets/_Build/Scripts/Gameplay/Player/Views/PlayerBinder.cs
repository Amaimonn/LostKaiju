using UnityEngine;

using LostKaiju.Utils;
using LostKaiju.Gameplay.Creatures.Features;
using LostKaiju.Gameplay.Configs;
using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Gameplay.Creatures.Presenters;

namespace LostKaiju.Gameplay.Player.Views
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

        // [Header("Behaviour")]
        // [SerializeField] private CreaturePresenterSO _presenterConfig;

        // protected CreaturePresenter CurrentBehaviour => _currentPresenterConfig == _presenterConfig ? _currentPresenter : SetPresenter(_presenterConfig);
        
        // protected CreaturePresenterSO _currentPresenterConfig;
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

        // private CreaturePresenter SetPresenter(CreaturePresenterSO config)
        // {
        //     Debug.Log("New Presenter set up");
        //     _currentPresenterConfig = config;
        //     _currentPresenter = config.GetPresenter();
        //     _currentPresenter.Bind(this);
        //     return _currentPresenter;
        // }
    }
}
