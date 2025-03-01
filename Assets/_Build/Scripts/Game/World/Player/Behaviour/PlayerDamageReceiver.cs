using R3;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public class PlayerDamageReceiver : DamageReceiver
    {
        public override Observable<int> OnDamageTaken => _onDamageTaken;
        
        private readonly Subject<int> _onDamageTaken = new();

        public override void TakeDamage(int amount)
        {
            _onDamageTaken.OnNext(amount);
        }
    }
}