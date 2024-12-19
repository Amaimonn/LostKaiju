using UnityEngine;

using LostKaiju.Architecture.Utils;
using LostKaiju.Creatures.CreatureFeatures;
using LostKaiju.Configs;
using LostKaiju.Creatures.Views;
using LostKaiju.Creatures.Presenters;

namespace LostKaiju.Player.Views
{
    public class PlayerBinder : CreatureBinder
    {    
        public override Rigidbody2D Rigidbody => _rigidbody;
        public override Animator Animator => _animator;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Flipper _flipper;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private CreaturePresenterSO _presenterConfig;

        protected CreaturePresenter CurrentBehaviour => _currentPresenterConfig == _presenterConfig ? _currentPresenter : SetPresenter(_presenterConfig);
        
        protected CreaturePresenterSO _currentPresenterConfig;
        protected CreaturePresenter _currentPresenter;
        protected Holder<ICreatureFeature> _features = new();

        private void Awake()
        {
            _features.Register<Flipper>(_flipper);
            _features.Register<GroundCheck>(_groundCheck);
        }

        private void Update()
        {
            CurrentBehaviour.UpdateLogic();
        }

        private void FixedUpdate()
        {
            CurrentBehaviour.FixedUpdateLogic();
        }

        private CreaturePresenter SetPresenter(CreaturePresenterSO config)
        {
            Debug.Log("New Presenter set up");
            _currentPresenterConfig = config;
            _currentPresenter = config.GetPresenter();
            _currentPresenter.Bind(this, _features);
            return _currentPresenter;
        }
    }
}
