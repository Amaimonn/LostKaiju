using UnityEngine;

using LostKaiju.Architecture.Utils;
using LostKaiju.Entities.EntityFeatures;
using LostKaiju.Configs;
using LostKaiju.Player.Behaviour;

namespace LostKaiju.Player.View
{
    public class PlayerCharacter : PlayerBinder
    {    
        public override Rigidbody2D PlayerRigidbody => _rigidbody;
        public override Animator PlayerAnimator => _animator;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Flipper _flipper;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private CharacterBehaviourSO _behaviourConfig;

        protected CharacterBehaviour CurrentBehaviour => _currentConfig == _behaviourConfig ? _currentModel : SetBehaviour(_behaviourConfig);
        
        protected CharacterBehaviourSO _currentConfig;
        protected CharacterBehaviour _currentModel;
        protected Holder<IEntityFeature> _features = new();

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

        private CharacterBehaviour SetBehaviour(CharacterBehaviourSO config)
        {
            Debug.Log("New behaviour set up");
            _currentConfig = config;
            _currentModel = config.GetModel();
            _currentModel.Bind(_rigidbody, _animator, _features);
            return _currentModel;
        }
    }
}
