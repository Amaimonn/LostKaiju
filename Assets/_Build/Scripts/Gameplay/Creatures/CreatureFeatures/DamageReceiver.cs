using UnityEngine;
using R3;

using LostKaiju.Gameplay.Creatures.DamageSystem;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public abstract class DamageReceiver : MonoBehaviour, IDamageable, ICreatureFeature
    {
        public abstract Observable<int> OnDamageTaken { get; }
        
        public abstract void TakeDamage(int amount);
    }
}