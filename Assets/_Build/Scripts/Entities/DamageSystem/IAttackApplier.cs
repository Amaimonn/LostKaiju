using UnityEngine;

namespace LostKaiju.Entities.DamageSystem
{
    public interface IAttackApplier
    {
        public void ApplyAttack(GameObject target);
    }
}
