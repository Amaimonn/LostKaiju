using UnityEngine;

namespace LostKaiju.Entities.EntityFeatures
{
    public abstract class Attacker : MonoBehaviour, IEntityFeature
    {
        public abstract void Attack();
    }
}
