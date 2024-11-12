using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Entities.DamageSystem;

namespace LostKaiju.Entities.EntityFeatures
{
    public class CollisionAtacker: Attacker
    {
        public Observable<Unit> OnAttack => _onAttack;

        [SerializeField] private Collider2D _collider;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private IAttackPathProcessor _attackPathProcessor;

        private readonly Subject<Unit> _onAttack = new();
        private IAttackApplier _attackApplier;
        private bool _isActive;

        public void Bind(IAttackApplier attackApplier)
        {
            _attackApplier = attackApplier;
        }

#region Attacker
        public override void Attack()
        {
            SetActive(true);
            _attackPathProcessor.Process(_collider);
            _onAttack.OnNext(Unit.Default);
        }
#endregion

#region MonoBehaviour
        private void Awake()
        {
            _collider.OnTriggerEnter2DAsObservable().Where(_ => _isActive == true).Subscribe(x => TryAttack(x.gameObject));
            SetActive(false);
            _attackPathProcessor.OnFinished.Where(x => x == true).Subscribe(_ => SetActive(false));
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
                damageable.TakeDamage(10);
                _attackApplier.ApplyAttack(target);
            }
        }
    }
}