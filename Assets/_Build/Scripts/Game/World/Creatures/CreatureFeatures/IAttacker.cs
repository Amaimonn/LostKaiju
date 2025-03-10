using R3;

namespace LostKaiju.Game.World.Creatures.Features
{
    public interface IAttacker : ICreatureFeature
    {
        public Observable<Unit> OnTargetAttacked { get; }
        public Observable<Unit> OnAttackCompleted { get; }
        
        public void Attack();
    }
}
