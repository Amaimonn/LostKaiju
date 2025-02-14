using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Shared
{
    public abstract class ScreenFactory
    {
        private readonly IRootUIBinder _rootUIBinder;

        public virtual void OpenScreen()
        {
        } 
    }

}