using UnityEngine;
using System.Collections;

namespace LostKaiju.Entities.Combat.AttackSystem 
{
    public interface IAttackPath
    {
        public IEnumerator Process(Collider2D collider);
    }
}