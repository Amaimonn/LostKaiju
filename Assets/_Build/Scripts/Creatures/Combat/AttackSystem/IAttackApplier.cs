using UnityEngine;

namespace LostKaiju.Creatures.Combat.AttackSystem
{
    public interface IAttackApplier
    {
        public void ApplyAttack(GameObject target);
    }
}
