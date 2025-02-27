using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay.MobileControls
{
    public class MobileControlsView : CanvasView<MobileControlsViewModel>
    {
        protected override CanvasOrder Order => CanvasOrder.First;
        
        protected override void OnBind(MobileControlsViewModel viewModel)
        {
        }
    }
}