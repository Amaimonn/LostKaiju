using R3;
using UnityEngine;

namespace LostKaiju.Entities.EntityFeatures
{
    public abstract class Attacker : MonoBehaviour, IAttacker
    {
        public abstract Observable<Unit> OnFinish { get; }

        public abstract void Attack();
    }
}
