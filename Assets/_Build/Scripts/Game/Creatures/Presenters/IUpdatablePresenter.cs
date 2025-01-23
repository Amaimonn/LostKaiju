namespace LostKaiju.Game.Creatures.Presenters
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