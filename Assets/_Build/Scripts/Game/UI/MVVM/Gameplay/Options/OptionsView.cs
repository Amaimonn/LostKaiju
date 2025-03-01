using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Providers.InputState;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class OptionsView : PopUpCanvasView<OptionsViewModel>, IInputBlocker
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