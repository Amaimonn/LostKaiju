using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class GameplayView : CanvasView<GameplayViewModel>
    {
        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(GameplayViewModel viewModel)
        {
        }
    }
}