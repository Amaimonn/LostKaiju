using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsView : ToolkitView<SettingsViewModel>
    {
        [SerializeField] private string _closeButtonName;
        [SerializeField] private string _applyButtonName;
        [SerializeField] private string _resetButtonName;
        [SerializeField] private string _sectionsRootClass;
        [SerializeField] private string _settingBarLabelClass;
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
            InitSections();
            _closeButton.RegisterCallbackOnce<ClickEvent>(Close);
            _applyButton.RegisterCallback<ClickEvent>(ApplyChanges);
            _resetButton.RegisterCallback<ClickEvent>(ResetSettings);
            _viewModel.OnOpenStateChanged.Skip(1).Subscribe(OnOpenStateChanged);
        }

        private void InitSections()
        {
            var sectionsRoot = Root.Q<VisualElement>(className: _sectionsRootClass);
            sectionsRoot.Clear();
            InitSoundSection(sectionsRoot);
            InitVideoSection(sectionsRoot);
        }
        
        private void InitSoundSection(VisualElement sectionsRoot)
        {
            var settingsData = _viewModel.SettingsData;
            var soundSection = new Tab()
            {
                label = settingsData.SoundSectionLabel,
            };
            sectionsRoot.Add(soundSection);
            var soundViewModel = _viewModel.SoundSettingsViewModel;

            var soundVolumeBar = _sliderSettingBarAsset.CloneTree();
            soundSection.Add(soundVolumeBar);
            var soundVolumeSlider = GetSlider(soundVolumeBar, settingsData.SoundVolumeData);
            soundVolumeSlider.RegisterCallback<ChangeEvent<float>>(e => soundViewModel.SetSoundVolume(e.newValue));
            _viewModel.SoundSettingsViewModel.SoundVolume.Subscribe(x => soundVolumeSlider.value = x);

            var sfxVolumeBar = _sliderSettingBarAsset.CloneTree();
            soundSection.Add(sfxVolumeBar);
            var sfxVolumeSlider = GetSlider(sfxVolumeBar, settingsData.SfxVolumeData);
            sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(e => soundViewModel.SetSfxVolume(e.newValue));
            _viewModel.SoundSettingsViewModel.SfxVolume.Subscribe(x => sfxVolumeSlider.value = x);
        }

        private void InitVideoSection(VisualElement sectionsRoot)
        {
            var settingsData = _viewModel.SettingsData;
            var videoSection = new Tab()
            {
                label = settingsData.VideoSectionLabel,
            };
            sectionsRoot.Add(videoSection);
            var videoViewModel = _viewModel.VideoSettingsViewModel;

            var brightnessBar = _sliderSettingBarAsset.CloneTree();
            videoSection.Add(brightnessBar);
            var brightnessSlider = GetSlider(brightnessBar, settingsData.BrightnessData);
            brightnessSlider.RegisterCallback<ChangeEvent<float>>(e => videoViewModel.SetBrightness(e.newValue));
            videoViewModel.Brightness.Subscribe(x => brightnessSlider.value = x);

            var bloomBar = _toggleSettingBarAsset.CloneTree();
            videoSection.Add(bloomBar);
            var bloomToggle = GetToggle(bloomBar, settingsData.IsHighBloomQualityData);
            bloomToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsHighBloomQuality(e.newValue));
            videoViewModel.IsHighBloomQuality.Subscribe(x => bloomToggle.value = x);

            var antiAliasingBar = _toggleSettingBarAsset.CloneTree();
            videoSection.Add(antiAliasingBar);
            var antiAliasingToggle = GetToggle(antiAliasingBar, settingsData.IsAntiAliasingEnabledData);
            antiAliasingToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsAntiAliasingEnabled(e.newValue));
            videoViewModel.IsAntiAliasingEnabled.Subscribe(x => antiAliasingToggle.value = x);
        }

        private Slider GetSlider(VisualElement sliderContainer, ISliderSettingData sliderSettingData)
        {
            var slider = sliderContainer.Q<Slider>();
            slider.lowValue = sliderSettingData.MinValue;
            slider.highValue = sliderSettingData.MaxValue;

            var label = sliderContainer.Q<Label>(className: _settingBarLabelClass);
            label.text = sliderSettingData.Label;

            return slider;
        }

        private Toggle GetToggle(VisualElement toggleContainer, IToggleSettingData toggleSettingData)
        {
            var toggle = toggleContainer.Q<Toggle>();
            var label = toggleContainer.Q<Label>(className: _settingBarLabelClass);
            label.text = toggleSettingData.Label;

            return toggle;
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
                Root.SetEnabled(false);
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
            _viewModel.ResetCurrentSectionSettings();
        }
    }
}