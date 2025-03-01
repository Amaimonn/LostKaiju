using UnityEngine;
using System.Collections;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem 
{
    public interface IAttackPath
    {
        public IEnumerator Process(Transform attackTransform);
    }
}