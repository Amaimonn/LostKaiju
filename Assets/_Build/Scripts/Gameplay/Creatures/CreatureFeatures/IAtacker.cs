using R3;

namespace LostKaiju.Gameplay.Creatures.CreatureFeatures
{
    public interface IAttacker : ICreatureFeature
    {
        public Observable<Unit> OnFinish { get; }
        
        public void Attack();
    }
}
