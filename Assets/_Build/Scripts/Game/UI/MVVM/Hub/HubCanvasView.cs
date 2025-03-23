using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubCanvasView : CanvasView<HubViewModel>
    {
        [SerializeField] private Button _openCampaignElement;
        [SerializeField] private Button _openHeroSelectionButton;
        [SerializeField] private Button _openSettingsButton;

        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(HubViewModel viewModel)
        {
            _openCampaignElement.onClick.AddListener(OpenCampaign);
            _openHeroSelectionButton.onClick.AddListener(OpenHeroSelection);
            _openSettingsButton.onClick.AddListener(OpenSettings);
        }

        private void OpenCampaign()
        {
            ViewModel.OpenCampaign();
        }

        private void OpenHeroSelection()
        {
            ViewModel.OpenHeroSelection();
        }

        private void OpenSettings()
        {
            ViewModel.OpenSettings();
        }
    }
}
