namespace LostKaiju.Game.World.Creatures.Presenters
{
    public interface IUpdatablePresenter
    {
        public virtual void UpdateLogic()
        {
        }

        public virtual void FixedUpdateLogic()
        {
        }
    }
}