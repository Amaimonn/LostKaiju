namespace LostKaiju.Models.UI.MVVM
{
    public abstract class CanvasView<T> : View<T> where T : IViewModel
    {
        public sealed override void Attach(IRootUI rootUI)
        {
            rootUI.Attach(gameObject);
        }
    }
}