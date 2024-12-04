using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Entities.DamageSystem;
using LostKaiju.Entities.EntityFeatures;

namespace LostKaiju.Entities.Combat.AttackSystem
{
    public class TriggerAttacker: Attacker
    {
        public Observable<Unit> OnTargetAttacked => _onTargetAttacked;
        public override Observable<Unit> OnFinish => _onFinish;

        [SerializeField] private Collider2D _collider;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private AttackProvider _attackProvider;
        [SerializeField] private IAttackPathProcessor _attackPathProcessor;

        private readonly Subject<Unit> _onTargetAttacked = new();
        private readonly Subject<Unit> _onFinish = new();
        private IAttackApplier _attackApplier;
        private bool _isActive;

        public void Bind(IAttackApplier attackApplier, IAttackPathProcessor attackPathProcessor = null)
        {
            _attackApplier = attackApplier;

            if (_attackApplier != null)
                _attackPathProcessor = attackPathProcessor;
            else
                _attackPathProcessor = new SingleAttackPathProcessor();
        }

#region Attacker
        public override void Attack()
        {
            SetActive(true);

            var currentAttack = _attackProvider.GetPath();

            StartCoroutine(_attackPathProcessor.Process(_collider, currentAttack));
        }
#endregion

#region MonoBehaviour
        private void Awake()
        {
            _collider.OnTriggerEnter2DAsObservable()
                .Where(collision => _isActive == true && (collision.gameObject.layer * _attackableMask) != 0)
                .Subscribe(x => TryAttack(x.gameObject));

            SetActive(false);
            _attackPathProcessor.OnFinished.Subscribe(_ => SetActive(false));
        }
#endregion

        private void SetActive(bool isActive)
        {
            _isActive = isActive;
            _collider.enabled = isActive;
        }

        private void TryAttack(GameObject target)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                _onTargetAttacked.OnNext(Unit.Default);
                damageable.TakeDamage(10);
                _attackApplier?.ApplyAttack(target);
            }
        }
    }
}