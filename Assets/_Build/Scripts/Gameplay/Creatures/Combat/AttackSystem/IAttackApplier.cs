using UnityEngine;

namespace LostKaiju.Game.Creatures.Combat.AttackSystem
{
    public interface IAttackApplier
    {
        public void ApplyAttack(GameObject target);
    }
}
