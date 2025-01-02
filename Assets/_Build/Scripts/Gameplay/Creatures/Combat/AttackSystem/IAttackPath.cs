using UnityEngine;
using System.Collections;

namespace LostKaiju.Gameplay.Creatures.Combat.AttackSystem 
{
    public interface IAttackPath
    {
        public IEnumerator Process(Collider2D collider);
    }
}