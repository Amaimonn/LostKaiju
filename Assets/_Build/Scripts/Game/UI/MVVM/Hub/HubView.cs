using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HubView : ToolkitView<HubViewModel>
    {
        [SerializeField] private string _openMissionsElementName;

        private VisualElement _openMissionsElement;

        protected override void OnBind(HubViewModel viewModel)
        {
            _openMissionsElement = Root.Q<VisualElement>(name: _openMissionsElementName);
            _openMissionsElement.RegisterCallback<ClickEvent>(OpenMissions);
        }

        private void OpenMissions(ClickEvent clickEvent)
        {
            _viewModel.OpenMissions();
        }
    }
}
