using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.UI.MVVM.Hub
{
    public class HubView : ToolkitView<HubViewModel>
    {
        [SerializeField] private string _startGameplayElementName;

        private VisualElement _startGameplayElement;

        protected override void OnBind(HubViewModel viewModel)
        {
            _startGameplayElement = _root.Q<VisualElement>(name: _startGameplayElementName);
            _startGameplayElement.RegisterCallback<ClickEvent>(StartGameplay);
        }

        private void StartGameplay(ClickEvent clickEvent)
        {
            _viewModel.StartGameplay();
        }
    }
}
