using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Creatures.Features
{
    public interface IAttacker : ICreatureFeature
    {
        public Observable<GameObject> OnTargetAttacked { get; }
        public Observable<Vector2> OnHitPositionSent { get; }
        public Observable<Unit> OnAttackCompleted { get; }
        
        public void Attack();
    }
}
