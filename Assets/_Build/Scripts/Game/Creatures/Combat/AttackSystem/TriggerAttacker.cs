using System.Collections;
using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Game.Creatures.DamageSystem;
using LostKaiju.Game.Creatures.Features;

namespace LostKaiju.Game.Creatures.Combat.AttackSystem
{
    public class TriggerAttacker: Attacker
    {
        public Observable<Unit> OnTargetAttacked => _onTargetAttacked;
        public override Observable<Unit> OnAttackCompleted => _onFinish;

        [SerializeField] private Collider2D _attackCollider;
        [SerializeField] private GameObject _attackGameObject;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private AttackProvider _attackProvider;

        private IAttackPathProcessor _attackPathProcessor = new SingleAttackPathProcessor(); // test
        private IAttackApplier _attackApplier;
        private readonly Subject<Unit> _onTargetAttacked = new();
        private readonly Subject<Unit> _onFinish = new();
        private bool _isDamagingModeActive;

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
            if (!_isDamagingModeActive)
            {
                SetDamagingModeActive(true);
                StartCoroutine(ProcessAttack());
            }
        }
#endregion

        private IEnumerator ProcessAttack()
        {
            var currentAttack = _attackProvider.GetPath();
            yield return _attackPathProcessor.Process(_attackGameObject.transform, currentAttack);
        }

#region MonoBehaviour
        private void Awake()
        {
            _attackCollider.OnTriggerEnter2DAsObservable()
                .Where(collision => _isDamagingModeActive == true && (collision.gameObject.layer & _attackableMask) != 0)
                .Subscribe(x => TryAttack(x.gameObject));

            SetDamagingModeActive(false);

            _attackPathProcessor.OnFinished.Subscribe(_ => {
                SetDamagingModeActive(false);
                _onFinish.OnNext(Unit.Default);
            });
        }
#endregion

        private void SetDamagingModeActive(bool isActive)
        {
            _isDamagingModeActive = isActive;
            _attackCollider.enabled = isActive;
            _attackGameObject.SetActive(isActive);
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