using R3;
using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public abstract class Attacker : MonoBehaviour, IAttacker
    {
        public abstract Observable<GameObject> OnTargetAttacked { get; }
        public abstract Observable<Vector2> OnHitPositionSent { get; }
        public abstract Observable<Unit> OnAttackCompleted { get; }

        public abstract void Attack();
    }
}
