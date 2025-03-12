using UnityEngine;
using R3;
using R3.Triggers;

using LostKaiju.Game.World.Creatures.DamageSystem;
using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    public class ShootAttacker: Attacker
    {
        public override Observable<GameObject> OnTargetAttacked => _onTargetAttacked;
        public override Observable<Vector2> OnHitPositionSent => _onHitPositionSent;
        public override Observable<Unit> OnAttackCompleted => _onFinish;

        [SerializeField] private Targeter _targeter;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private LayerMask _attackableMask;
        [SerializeField] private ShootDataSO _shootData;

        private IAttackApplier _attackApplier;
        private readonly Subject<GameObject> _onTargetAttacked = new();
        private readonly Subject<Vector2> _onHitPositionSent = new();
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
                    .Where(collision => ((1 << collision.gameObject.layer) & _attackableMask) != 0)
                    .Subscribe(x => {
                        TryAttackWithProjectile(x.gameObject, projectile);
                        projectile.PlaySparks();
                        Destroy(projectile.gameObject);
                    });
                    
                projectile.WithSpeed(_shootData.Speed)
                    .WithDestination(targetPosition)
                    .Shoot();
            }
        }
#endregion

        private void TryAttackWithProjectile(GameObject target, Projectile projectile)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                _onTargetAttacked.OnNext(target);
                _onHitPositionSent.OnNext(projectile.transform.position);
                damageable.TakeDamage(_shootData.Damage);
                _attackApplier?.ApplyAttack(target);
            }
        }
    }
}