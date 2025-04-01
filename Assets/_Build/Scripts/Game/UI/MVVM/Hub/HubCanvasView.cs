using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubCanvasView : CanvasView<HubViewModel>
    {
        [Header("UI")]
        [SerializeField] private Button _openCampaignElement;
        [SerializeField] private Button _openHeroSelectionButton;
        [SerializeField] private Button _openSettingsButton;

        [Header("SFX"), Space(4)]
        [SerializeField] private AudioClip _buttonClickSFX;
        private AudioPlayer _audioPlayer;

        protected override CanvasOrder Order => CanvasOrder.First;

        public void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        protected override void OnBind(HubViewModel viewModel)
        {
            _openCampaignElement.onClick.AddListener(OpenCampaign);
            _openHeroSelectionButton.onClick.AddListener(OpenHeroSelection);
            _openSettingsButton.onClick.AddListener(OpenSettings);
        }

        private void OpenCampaign()
        {
            ViewModel.OpenCampaign();
            PlayButtonClickSFX();
        }

        private void OpenHeroSelection()
        {
            ViewModel.OpenHeroSelection();
            PlayButtonClickSFX();
        }

        private void OpenSettings()
        {
            ViewModel.OpenSettings();
            PlayButtonClickSFX();
        }

        private void PlayButtonClickSFX()
        {
            _audioPlayer.PlayRandomPitchSFX(_buttonClickSFX);
        }
    }
}
