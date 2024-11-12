using R3;
using UnityEngine;

namespace LostKaiju.Entities.DamageSystem
{
    public interface IAttackPathProcessor
    {
        public Observable<bool> OnFinished { get; }

        public void Process(Collider2D areaCollider);
    }
}