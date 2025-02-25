namespace LostKaiju.Game.Creatures.Features
{
    public interface IGroundCheck : ICreatureFeature
    {
        public abstract bool IsGrounded { get; }
    }
} 
