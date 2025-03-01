using System.Collections;
using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem 
{
    public interface IAttackPathProcessor
    {
        public Observable<Unit> OnFinished { get; }

        public IEnumerator Process(Transform attackTransform, IAttackPath attackPath);
    }
}