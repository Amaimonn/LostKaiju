using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class ExitPopUpView : PopUpCanvasView<ExitPopUpViewModel>
    {
        [Header("UI")]
        [SerializeField] private Button _confirmExitButton;

        [Header("SFX"), Space(4)]
        [SerializeField] private AudioClip _closingSFX;

        protected override CanvasOrder Order => CanvasOrder.First;

#region PopUpCanvasView
        public override void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        protected override void OnBind(ExitPopUpViewModel viewModel)
        {
            base.OnBind(viewModel);
            _confirmExitButton.onClick.AddListener(viewModel.ConfirmExit);
        }

        protected override void OnClosing()
        {
            PlayClosingSFX();
            base.OnClosing();
        }
#endregion

        private void PlayClosingSFX()
        {
            _audioPlayer.PlaySFX(_closingSFX);
        }
    }
}