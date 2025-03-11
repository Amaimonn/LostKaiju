namespace LostKaiju.Boilerplates.UI.MVVM
{
    public interface IView<T> where T : IViewModel
    {
        public void Bind(T viewModel);
    }
}