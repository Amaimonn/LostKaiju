namespace LostKaiju.Boilerplates.UI.MVVM
{
    public abstract class CanvasView<T> : View<T> where T : IViewModel
    {
        public sealed override void Attach(IRootUI rootUI)
        {
            rootUI.Attach(gameObject);
        }

        public sealed override void Detach(IRootUI rootUI)
        {
            rootUI.Detach(gameObject);
        }
    }
}