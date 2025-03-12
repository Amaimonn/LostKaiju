using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    public interface IForceable
    {
        public void Force(Vector2 origin, float force);
    }
}