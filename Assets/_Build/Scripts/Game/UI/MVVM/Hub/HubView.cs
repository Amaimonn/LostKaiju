using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubView : ToolkitView<HubViewModel>
    {
        [SerializeField] private string _openMissionsElementName;
        [SerializeField] private string _openHeroSelectionButtonName;

        private VisualElement _openMissionsElement;
        private Button _openHeroSelectionButton;

        protected override void OnBind(HubViewModel viewModel)
        {
            _openMissionsElement = Root.Q<VisualElement>(name: _openMissionsElementName);
            _openMissionsElement.RegisterCallback<ClickEvent>(OpenMissions);

            _openHeroSelectionButton = Root.Q<Button>(name: _openHeroSelectionButtonName);
            _openHeroSelectionButton.RegisterCallback<ClickEvent>(OpenHeroSelection);
        }

        private void OpenMissions(ClickEvent clickEvent)
        {
            ViewModel.OpenMissions();
        }

        private void OpenHeroSelection(ClickEvent clickEvent)
        {
            ViewModel.OpenHeroSelection();
        }
    }
}
