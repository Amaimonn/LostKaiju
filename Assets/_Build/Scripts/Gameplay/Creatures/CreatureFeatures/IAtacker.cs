using R3;

namespace LostKaiju.Gameplay.Creatures.Features
{
    public interface IAttacker : ICreatureFeature
    {
        public Observable<Unit> OnFinish { get; }
        
        public void Attack();
    }
}
