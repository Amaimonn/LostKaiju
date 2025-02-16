using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsView : ToolkitView<SettingsViewModel>
    {
        [SerializeField] private string _closeButtonName;
        [SerializeField] private string _applyButtonName;
        [SerializeField] private string _resetButtonName;
        [SerializeField] private VisualTreeAsset _sliderSettingBarAsset;
        [SerializeField] private VisualTreeAsset _toggleSettingBarAsset;

        private Button _closeButton;
        private Button _applyButton;
        private Button _resetButton;
        private bool _isClosing = false;

        protected override void OnAwake()
        {
            _closeButton = Root.Q<Button>(name: _closeButtonName);
            _applyButton = Root.Q<Button>(name: _applyButtonName);
            _resetButton = Root.Q<Button>(name: _resetButtonName);
        }

        protected override void OnBind(SettingsViewModel viewModel)
        {
            _closeButton.RegisterCallbackOnce<ClickEvent>(Close);
            _applyButton.RegisterCallbackOnce<ClickEvent>(ApplyChanges);
            _resetButton.RegisterCallbackOnce<ClickEvent>(ResetSettings);
            _viewModel.OnOpenStateChanged.Skip(1).Subscribe(OnOpenStateChanged);
        }

        private void OnOpenStateChanged(bool isOpened)
        {
            if (isOpened)
                OnOpened();
            else
                OnClosed();

            void OnOpened()
            {
                StartCoroutine(OpenAnimation());

                IEnumerator OpenAnimation()
                {
                    yield return null;
                    // _contentElement.RemoveFromClassList($"{_contentStyleName}--disabled");
                    // _panelWhiteBackground.AddToClassList($"{_panelWhiteBackgroundStyleName}--enabled");
                    Debug.Log("Settings: opened");
                }
            }

            void OnClosed()
            {
                _isClosing = true;
                if (_isClosing)
                    _viewModel.CompleteClosing(); // TODO: replace it to the TransitionEndEvent
                // _contentElement.AddToClassList($"{_contentStyleName}--disabled");
                // _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundStyleName}--enabled");
                Debug.Log("Settings: closed");
            }
        }

        private void Close(ClickEvent clickEvent)
        {
            _viewModel.Close();
        }

        private void ApplyChanges(ClickEvent clickEvent)
        {
            _viewModel.ApplyChanges();
        }

        private void ResetSettings(ClickEvent clickEvent)
        {
            _viewModel.ResetSettings();
        }
    }
}