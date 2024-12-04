using UnityEngine;

namespace LostKaiju.Entities.Combat.AttackSystem
{
    public interface IAttackApplier
    {
        public void ApplyAttack(GameObject target);
    }
}
