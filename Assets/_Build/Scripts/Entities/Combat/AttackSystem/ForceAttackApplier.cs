using LostKaiju.Entities.DamageSystem;
using UnityEngine;

namespace LostKaiju.Entities.Combat.AttackSystem
{
    public class ForceAttackApplier : IAttackApplier
    {
        private Transform _forceOrigin;
        private float _strength;

        public ForceAttackApplier(Transform forceOrigin, float strength)
        {
            _forceOrigin = forceOrigin;
            _strength = strength;
        }

        public void ApplyAttack(GameObject target)
        {
            if (target.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                var forceDirection = target.transform.position - _forceOrigin.position;
                rigidbody.AddForce(forceDirection * _strength, ForceMode.Impulse);
            }
        }
    }
}