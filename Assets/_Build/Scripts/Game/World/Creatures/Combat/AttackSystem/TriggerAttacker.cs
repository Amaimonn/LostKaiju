using System.Collections;
using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Game.World.Creatures.DamageSystem;
using LostKaiju.Game.World.Creatures.Features;
using System.Collections.Generic;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    public class TriggerAttacker: Attacker
    {
        public override Observable<GameObject> OnTargetAttacked => _onTargetAttacked;
        public override Observable<Vector2> OnHitPositionSent => _onHitPositionSent;
        public override Observable<Unit> OnAttackCompleted => _onFinish;

        [SerializeField] private Collider2D _attackCollider;
        [SerializeField] private GameObject _attackGameObject;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private AttackProvider _attackProvider;
        [SerializeField] private AttackForceApplier _forceAttackApplier;

        private IAttackPathProcessor _attackPathProcessor = new SingleAttackPathProcessor(); // test
        private IAttackApplier _attackApplier;
        private readonly Subject<GameObject> _onTargetAttacked = new();
        private readonly Subject<Vector2> _onHitPositionSent = new();
        private readonly Subject<Unit> _onFinish = new();
        private bool _isDamagingModeActive;
        private readonly HashSet<IDamageable> _attackedSet = new();

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
                .Where(collision => _isDamagingModeActive == true && (1 << collision.gameObject.layer & _attackableMask) != 0)
                .Subscribe(x => TryAttack(x));

            SetDamagingModeActive(false);

            _attackPathProcessor.OnFinished.Subscribe(_ => {
                SetDamagingModeActive(false);
                _onFinish.OnNext(Unit.Default);
            });
        }

        private void OnDisable()
        {
            SetDamagingModeActive(false);
            _onFinish.OnNext(Unit.Default);
            _attackGameObject.SetActive(false);
            StopCoroutine(ProcessAttack());
        }
#endregion

        private void SetDamagingModeActive(bool isActive)
        {
            _isDamagingModeActive = isActive;
            _attackCollider.enabled = isActive;
            _attackGameObject.SetActive(isActive);
            _attackedSet.Clear();
        }

        private void TryAttack(Collider2D collider)
        {
            if (collider.gameObject.TryGetComponent(out IDamageable damageable) && !_attackedSet.Contains(damageable))
            {
                _attackedSet.Add(damageable);
                _onTargetAttacked.OnNext(collider.gameObject);
                _onHitPositionSent.OnNext(collider.ClosestPoint(_attackGameObject.transform.position));
                damageable.TakeDamage(10);
                _forceAttackApplier?.TryApplyForce(collider.gameObject);
                _attackApplier?.ApplyAttack(collider.gameObject);
            }
        }
    }
}