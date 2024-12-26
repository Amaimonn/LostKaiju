using R3;
using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.CreatureFeatures
{
    public abstract class Attacker : MonoBehaviour, IAttacker
    {
        public abstract Observable<Unit> OnFinish { get; }

        public abstract void Attack();
    }
}
