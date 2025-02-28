using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class ExitPopUpView : PopUpCanvasView<ExitPopUpViewModel>
    {
        [SerializeField] private Button _confirmExitButton;

        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(ExitPopUpViewModel viewModel)
        {
            base.OnBind(viewModel);
            _confirmExitButton.onClick.AddListener(viewModel.ConfirmExit);
        }
    }
}