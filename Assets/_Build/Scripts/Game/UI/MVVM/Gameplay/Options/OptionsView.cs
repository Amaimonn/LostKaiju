using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class OptionsView : PopUpCanvasView<OptionsViewModel>
    {
        [SerializeField] private Button _openSettingsButton;
        [SerializeField] private Button _openExitPopUpButton;

        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(OptionsViewModel viewModel)
        {
            base.OnBind(viewModel);
            _openSettingsButton.onClick.AddListener(ViewModel.OpenSettings);
            _openExitPopUpButton.onClick.AddListener(ViewModel.OpenExitPopUp);
        }
    }
}