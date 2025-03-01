namespace LostKaiju.Game.World.Creatures.Features
{
    public interface IGroundCheck : ICreatureFeature
    {
        public abstract bool IsGrounded { get; }
    }
} 
