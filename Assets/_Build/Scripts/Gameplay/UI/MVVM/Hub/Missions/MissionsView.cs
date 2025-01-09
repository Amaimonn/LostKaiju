using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

namespace LostKaiju.Gameplay.UI.MVVM.Hub
{
    public class MissionsView : ToolkitView<MissionsViewModel>
    {
        [SerializeField] private string _contentElementName;
        [SerializeField] private string _startButtonName;
        [SerializeField] private string _closeButtonName;
        [SerializeField] private string _contentStyleName;

        private Button _startButton;
        private Button _closeButton;
        private VisualElement _contentElement;
        private bool _isGameplayStarted = false;
        private bool _isClosing = false;

        protected override void OnAwake()
        {
            _contentElement = _root.Q<VisualElement>(name: _contentElementName);
            _startButton = _root.Q<Button>(name: _startButtonName);
            _closeButton = _root.Q<Button>(name: _closeButtonName);

            _contentElement.AddToClassList($"{_contentStyleName}--disabled");
            _contentElement.RegisterCallback<TransitionEndEvent>(_ => {
                if (_isClosing)
                    _viewModel.CompleteClose();
            });
        }

        protected override void OnBind(MissionsViewModel viewModel)
        {
            _startButton.RegisterCallback<ClickEvent>(_ => StartGameplay());
            _closeButton.RegisterCallback<ClickEvent>(_ =>  Close());

            _viewModel.OnOpenStateChanged.Skip(1).Subscribe(e => OnOpedStateChanged(e));
        }

        private void StartGameplay()
        {
            if (_isGameplayStarted)
            {
                return;
            }
            _viewModel.StartGameplay();
            _isGameplayStarted = true;
        }

        private void Close()
        {
            Debug.Log("Missions: close button clicked");
            _viewModel.Close();
        }

        private void OnOpedStateChanged(bool isOpened)
        {
            if (isOpened)
                OnOpened();
            else
                OnClosed();
        }

        private void OnOpened()
        {
            StartCoroutine(OpenAnimation());
        }

        private IEnumerator OpenAnimation()
        {
            yield return null;
            _contentElement.RemoveFromClassList($"{_contentStyleName}--disabled");
            Debug.Log("Missions: opened");
        }

        private void OnClosed()
        {
            _isClosing = true;
            _contentElement.AddToClassList($"{_contentStyleName}--disabled");
            Debug.Log("Missions: closed");
        }
    }
}
