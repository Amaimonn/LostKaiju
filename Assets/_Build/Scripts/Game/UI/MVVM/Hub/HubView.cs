using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubView : ToolkitView<HubViewModel>
    {
        [SerializeField] private string _openMissionsElementName;
        [SerializeField] private string _openHeroSelectionButtonName;
        [SerializeField] private string _openSettingsButtonName;

        private VisualElement _openCampaignElement;
        private Button _openHeroSelectionButton;
        private Button _openSettingsButton;

        protected override void OnBind(HubViewModel viewModel)
        {
            _openCampaignElement = Root.Q<VisualElement>(name: _openMissionsElementName);
            _openCampaignElement.RegisterCallback<ClickEvent>(OpenCampaign);

            _openHeroSelectionButton = Root.Q<Button>(name: _openHeroSelectionButtonName);
            _openHeroSelectionButton.RegisterCallback<ClickEvent>(OpenHeroSelection);

            _openSettingsButton = Root.Q<Button>(name: _openSettingsButtonName);
            _openSettingsButton?.RegisterCallback<ClickEvent>(OpenSettings);
        }

        private void OpenCampaign(ClickEvent clickEvent)
        {
            ViewModel.OpenCampaign();
        }

        private void OpenHeroSelection(ClickEvent clickEvent)
        {
            ViewModel.OpenHeroSelection();
        }

        private void OpenSettings(ClickEvent clickEvent)
        {
            ViewModel.OpenSettings();
        }
    }
}
