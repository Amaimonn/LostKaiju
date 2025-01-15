using R3;
using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public abstract class Attacker : MonoBehaviour, IAttacker
    {
        public abstract Observable<Unit> OnAttackCompleted { get; }

        public abstract void Attack();
    }
}
