using R3;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Creatures.Combat.Defence
{
    public class RawDamageReceiver : DamageReceiver
    {
        public override Observable<int> OnDamageTaken => _onDamageTaken;
        
        protected readonly Subject<int> _onDamageTaken = new();

        public override void TakeDamage(int amount)
        {
            _onDamageTaken.OnNext(amount);
        }
    }
}