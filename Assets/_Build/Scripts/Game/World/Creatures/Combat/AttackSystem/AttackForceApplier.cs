using UnityEngine;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    [System.Serializable]
    public class AttackForceApplier
    {
        [SerializeField] private Transform _forceOrigin;
        [SerializeField] private ForceAttackDataSO _data;

        public void TryApplyForce(GameObject target)
        {
            if (target.TryGetComponent<IPusher>(out var forceable))
                forceable.Push(_forceOrigin.position, _data.Force);
        }

        public void TryApplyForceFromOrigin(GameObject target, Vector2 customOrigin)
        {
            if (target.TryGetComponent<IPusher>(out var forceable))
                forceable.Push(customOrigin, _data.Force);
        }
    }
}