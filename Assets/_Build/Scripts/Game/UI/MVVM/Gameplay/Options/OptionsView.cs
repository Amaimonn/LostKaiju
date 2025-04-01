using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Providers.InputState;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class OptionsView : PopUpCanvasView<OptionsViewModel>, IInputBlocker
    {
        [Header("UI")]
        [SerializeField] private Button _openSettingsButton;
        [SerializeField] private Button _openExitPopUpButton;

        [Header("SFX"), Space(4)]
        [SerializeField] private AudioClip _buttonClickSFX;

        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(OptionsViewModel viewModel)
        {
            base.OnBind(viewModel);
            _openSettingsButton.onClick.AddListener(OpenSettings);
            _openExitPopUpButton.onClick.AddListener(OpenExitPopUp);
        }

        private void OpenSettings()
        {
            PlayButtonClickSFX();
            ViewModel.OpenSettings();
        }

        private void OpenExitPopUp()
        {
            PlayButtonClickSFX();
            ViewModel.OpenExitPopUp();
        }

        private void PlayButtonClickSFX()
        {
            _audioPlayer.PlayRandomPitchSFX(_buttonClickSFX);
        }
    }
}