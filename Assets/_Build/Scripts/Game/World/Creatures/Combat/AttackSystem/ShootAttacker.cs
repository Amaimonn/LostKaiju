using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Game.World.Creatures.DamageSystem;
using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    public class ShootAttacker: Attacker
    {
        public override Observable<Unit> OnTargetAttacked => _onTargetAttacked;
        public override Observable<Unit> OnAttackCompleted => _onFinish;

        [SerializeField] private Targeter _targeter;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private ShootDataSO _shootData;

        private IAttackApplier _attackApplier;
        private readonly Subject<Unit> _onTargetAttacked = new();
        private readonly Subject<Unit> _onFinish = new();

#region Attacker
        public override void Attack()
        {
            if (_targeter.IsTargeting)
            {
                var targetPosition = _targeter.GetTargetPosition();
                targetPosition.y += 1;
                
                var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
                projectile.Collider.OnTriggerEnter2DAsObservable()
                    .Where(collision => (1 << collision.gameObject.layer & _attackableMask) != 0)
                    .Subscribe(x => {
                        TryAttack(x.gameObject);
                        Destroy(projectile.gameObject);
                    });
                    
                projectile.WithSpeed(_shootData.Speed)
                    .WithDestination(targetPosition)
                    .Shoot();
            }
        }
#endregion

        private void TryAttack(GameObject target)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                _onTargetAttacked.OnNext(Unit.Default);
                damageable.TakeDamage(_shootData.Damage);
                _attackApplier?.ApplyAttack(target);
            }
        }
    }
}