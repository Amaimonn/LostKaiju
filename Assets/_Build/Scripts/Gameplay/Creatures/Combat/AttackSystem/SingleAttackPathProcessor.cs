using System.Collections;
using UnityEngine;
using R3;

namespace LostKaiju.Gameplay.Creatures.Combat.AttackSystem
{
    public class SingleAttackPathProcessor : IAttackPathProcessor
    {
        public Observable<Unit> OnFinished => _onFinished;

        private Subject<Unit> _onFinished = new();

        public IEnumerator Process(Collider2D attackCollider, IAttackPath attackPath)
        {
            yield return attackPath.Process(attackCollider);
            _onFinished.OnNext(Unit.Default);
        }
    }
}