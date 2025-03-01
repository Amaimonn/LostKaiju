using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    public interface IAttackApplier
    {
        public void ApplyAttack(GameObject target);
    }
}
