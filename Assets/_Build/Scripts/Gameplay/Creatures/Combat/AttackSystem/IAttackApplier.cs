using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Combat.AttackSystem
{
    public interface IAttackApplier
    {
        public void ApplyAttack(GameObject target);
    }
}
