using UnityEngine;
using R3;

using LostKaiju.Game.Creatures.DamageSystem;

namespace LostKaiju.Game.Creatures.Features
{
    public abstract class DamageReceiver : MonoBehaviour, IDamageable, ICreatureFeature
    {
        public abstract Observable<int> OnDamageTaken { get; }
        
        public abstract void TakeDamage(int amount);
    }
}