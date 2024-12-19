using UnityEngine;
using System.Collections;

namespace LostKaiju.Creatures.Combat.AttackSystem 
{
    public interface IAttackPath
    {
        public IEnumerator Process(Collider2D collider);
    }
}