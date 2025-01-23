using UnityEngine;
using System.Collections;

namespace LostKaiju.Game.Creatures.Combat.AttackSystem 
{
    public interface IAttackPath
    {
        public IEnumerator Process(Transform attackTransform);
    }
}