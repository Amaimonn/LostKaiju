using R3;

namespace LostKaiju.Game.Creatures.Features
{
    public interface IAttacker : ICreatureFeature
    {
        public Observable<Unit> OnAttackCompleted { get; }
        
        public void Attack();
    }
}
