using UnityEngine;
using R3;

namespace LostKaiju.Game.Creatures.Features
{
    public abstract class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        public abstract Observable<int> OnDamageTaken { get; }
        
        public abstract void TakeDamage(int amount);
    }
}