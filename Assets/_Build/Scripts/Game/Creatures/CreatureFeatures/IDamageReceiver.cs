using R3;

using LostKaiju.Game.Creatures.DamageSystem;

namespace LostKaiju.Game.Creatures.Features
{
    public interface IDamageReceiver : IDamageable, ICreatureFeature
    {
        public Observable<int> OnDamageTaken { get; }
    }
}