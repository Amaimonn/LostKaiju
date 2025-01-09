using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Gameplay.UI.MVVM.Hub
{
    public class HubView : ToolkitView<HubViewModel>
    {
        [SerializeField] private string _openMissionsElementName;

        private VisualElement _openMissionsElement;

        protected override void OnBind(HubViewModel viewModel)
        {
            _openMissionsElement = _root.Q<VisualElement>(name: _openMissionsElementName);
            _openMissionsElement.RegisterCallback<ClickEvent>(OpenMissions);
        }

        private void OpenMissions(ClickEvent clickEvent)
        {
            _viewModel.OpenMissions();
        }
    }
}
