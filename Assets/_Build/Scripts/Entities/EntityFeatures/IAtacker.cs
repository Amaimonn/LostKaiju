using R3;

namespace LostKaiju.Entities.EntityFeatures
{
    public interface IAttacker : IEntityFeature
    {
        public Observable<Unit> OnFinish { get; }
        
        public void Attack();
    }
}
