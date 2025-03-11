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
            _direction = Vector3.Normalize((Vector2)(targetPosition - transform.position));
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

            _rigidbody.SetRotation(angle);
            _rigidbody.linearVelocity = _direction * _speed;
        }
    }
}