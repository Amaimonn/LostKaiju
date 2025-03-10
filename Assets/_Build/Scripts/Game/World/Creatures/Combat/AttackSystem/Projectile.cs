using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem 
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        public Collider2D Collider => _collider;

        [SerializeField] private Collider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed;

        private Vector3 _direction;

        public Projectile WithDirection(Vector3 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public Projectile WithDestination(Vector3 targetPosition)
        {
            _direction = (targetPosition - transform.position).normalized;
            return this;
        }

        public Projectile WithSpeed(float speed)
        {
            _speed = speed;
            return this;
        }

        public void Shoot()
        {
            var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            _rigidbody.AddForce(_speed * _direction, ForceMode2D.Impulse);
        }
    }
}