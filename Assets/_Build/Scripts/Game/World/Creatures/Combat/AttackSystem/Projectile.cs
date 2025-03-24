using UnityEngine;

using LostKaiju.Game.World.Creatures.DamageSystem;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem 
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour, IDamageable
    {
        public Collider2D Collider => _collider;

        [SerializeField] private Collider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private ParticleSystem _hitParticles;

        private Vector2 _direction;

        public Projectile WithDirection(Vector2 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public Projectile WithDestination(Vector3 targetPosition)
        {
            _direction = ((Vector2)(targetPosition - transform.position)).normalized;
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
            _rigidbody.velocity = _direction * _speed;
        }

        public void PlaySparks()
        {
            Instantiate(_hitParticles, transform.position, Quaternion.identity);
        }

        public void TakeDamage(int amount)
        {
            PlaySparks();
            Destroy(gameObject);
        }
    }
}