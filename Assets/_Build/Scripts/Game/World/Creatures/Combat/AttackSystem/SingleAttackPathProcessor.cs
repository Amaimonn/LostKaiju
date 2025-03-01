using System.Collections;
using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    public class SingleAttackPathProcessor : IAttackPathProcessor
    {
        public Observable<Unit> OnFinished => _onFinished;

        private Subject<Unit> _onFinished = new();

        public IEnumerator Process(Transform attackTransform, IAttackPath attackPath)
        {
            yield return attackPath.Process(attackTransform);
            _onFinished.OnNext(Unit.Default);
        }
    }
}