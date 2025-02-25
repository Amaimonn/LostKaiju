namespace LostKaiju.Game.Creatures.Features
{
    public interface IFlipper : ICreatureFeature
    {
        public abstract bool IsLookingToTheRight { get; }
        public abstract void LookRight(bool isTrue);
    }
}