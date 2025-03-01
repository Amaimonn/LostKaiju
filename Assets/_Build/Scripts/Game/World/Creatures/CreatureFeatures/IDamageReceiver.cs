using R3;

using LostKaiju.Game.World.Creatures.DamageSystem;

namespace LostKaiju.Game.World.Creatures.Features
{
    public interface IDamageReceiver : IDamageable, ICreatureFeature
    {
        public Observable<int> OnDamageTaken { get; }
    }
}