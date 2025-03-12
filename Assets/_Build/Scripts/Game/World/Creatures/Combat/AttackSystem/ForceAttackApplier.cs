using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    [System.Serializable]
    public class ForceAttackApplier : IAttackApplier
    {
        [SerializeField] private Transform _forceOrigin;
        [SerializeField] private float _strength;

        public ForceAttackApplier(Transform forceOrigin, float strength)
        {
            _forceOrigin = forceOrigin;
            _strength = strength;
        }

        public void ApplyAttack(GameObject target)
        {
            if (target.TryGetComponent<IForceable>(out var forceable))
                forceable.Force(_forceOrigin.position, _strength);
        }
    }
}