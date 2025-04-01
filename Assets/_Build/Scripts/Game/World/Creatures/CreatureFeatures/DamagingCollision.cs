using UnityEngine;
using R3;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class DamagingCollision : MonoBehaviour, ICreatureFeature
    {
        public Observable<GameObject> OnTargetAttacked => _onTargetAttacked;
        public Observable<Vector2> OnHitPositionSent => _onHitPositionSent;

        [SerializeField] private int _damage = 10;
        [SerializeField] private int _pushForce = 5;
        [SerializeField] private LayerMask _attackableMask;

        private readonly Subject<GameObject> _onTargetAttacked = new();
        private readonly Subject<Vector2> _onHitPositionSent = new();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & _attackableMask) != 0 &&
                 collision.gameObject.TryGetComponent<ICreatureBinder>(out var creature))
            {
                if (creature.Features.TryResolve<IDamageReceiver>(out var damageReceiver))
                {
                    _onTargetAttacked.OnNext(collision.gameObject);
                    _onHitPositionSent.OnNext(collision.collider.ClosestPoint(transform.position));
                    damageReceiver.TakeDamage(_damage);
                    if (collision.gameObject.TryGetComponent<IPusher>(out var pusher))
                        pusher.Push(transform.position, _pushForce);
                    
                }
            }
        }
    }
}