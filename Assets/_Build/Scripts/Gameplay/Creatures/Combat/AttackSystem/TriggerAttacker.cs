using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Gameplay.Creatures.DamageSystem;
using LostKaiju.Gameplay.Creatures.Features;
using System.Collections;

namespace LostKaiju.Gameplay.Creatures.Combat.AttackSystem
{
    public class TriggerAttacker: Attacker
    {
        public Observable<Unit> OnTargetAttacked => _onTargetAttacked;
        public override Observable<Unit> OnAttackCompleted => _onFinish;

        [SerializeField] private Collider2D _collider;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private AttackProvider _attackProvider;

        private IAttackPathProcessor _attackPathProcessor = new SingleAttackPathProcessor(); // test
        private IAttackApplier _attackApplier;
        private readonly Subject<Unit> _onTargetAttacked = new();
        private readonly Subject<Unit> _onFinish = new();
        private bool _isActive;
        private bool _isAttacking = false;

        // public void Bind(IAttackApplier attackApplier, IAttackPathProcessor attackPathProcessor = null)
        // {
        //     _attackApplier = attackApplier;

        //     if (_attackApplier != null)
        //         _attackPathProcessor = attackPathProcessor;
        //     else
        //         _attackPathProcessor = new SingleAttackPathProcessor();
            
        //     _attackPathProcessor.OnFinished.Subscribe(_ => SetDamagingMode(false));
        // }

#region Attacker
        public override void Attack()
        {
            SetDamagingMode(true);
            if (!_isAttacking)
                StartCoroutine(ProcessAttack());
        }
#endregion

        private IEnumerator ProcessAttack()
        {
            _isAttacking = true;
            var currentAttack = _attackProvider.GetPath();
            yield return _attackPathProcessor.Process(_collider, currentAttack);
            _isAttacking = false;
        }

#region MonoBehaviour
        private void Awake()
        {
            _collider.OnTriggerEnter2DAsObservable()
                .Where(collision => _isActive == true && (collision.gameObject.layer & _attackableMask) != 0)
                .Subscribe(x => TryAttack(x.gameObject));

            SetDamagingMode(false);

            _attackPathProcessor.OnFinished.Subscribe(_ => {
                SetDamagingMode(false);
                _onFinish.OnNext(Unit.Default);
            });
        }
#endregion

        private void SetDamagingMode(bool isActive)
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