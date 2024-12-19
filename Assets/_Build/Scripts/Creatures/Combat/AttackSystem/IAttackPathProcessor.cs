using System.Collections;
using UnityEngine;
using R3;

namespace LostKaiju.Creatures.Combat.AttackSystem 
{
    public interface IAttackPathProcessor
    {
        public Observable<Unit> OnFinished { get; }

        public IEnumerator Process(Collider2D attackCollider, IAttackPath attackPath);
    }
}