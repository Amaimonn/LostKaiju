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
            ViewModel.OnOpenStateChanged.Skip(1).Subscribe(OnOpenStateChanged);
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
            var soundViewModel = ViewModel.SoundSettingsViewModel;
            var settingsData = ViewModel.SettingsData;
            var soundSection = new Tab()
            {
                label = settingsData.SoundSectionLabel,
            };
            soundSection.selected += _ => ViewModel.SelectSoundSection();
            sectionsRoot.Add(soundSection);

            var soundVolumeSlider = CreateSlider(settingsData.SoundVolumeData, soundSection);
            soundVolumeSlider.RegisterCallback<ChangeEvent<float>>(e => soundViewModel.SetSoundVolume(e.newValue));
            ViewModel.SoundSettingsViewModel.SoundVolume.Subscribe(x => soundVolumeSlider.value = x);

            var sfxVolumeSlider = CreateSlider(settingsData.SfxVolumeData, soundSection);
            sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(e => soundViewModel.SetSfxVolume(e.newValue));
            ViewModel.SoundSettingsViewModel.SfxVolume.Subscribe(x => sfxVolumeSlider.value = x);
        }

        private void InitVideoSection(VisualElement sectionsRoot)
        {
            var videoViewModel = ViewModel.VideoSettingsViewModel;
            var settingsData = ViewModel.SettingsData;
            var videoSection = new Tab()
            {
                label = settingsData.VideoSectionLabel,
            };
            videoSection.selected += _ => ViewModel.SelectVideoSection();
            sectionsRoot.Add(videoSection);

            var brightnessSlider = CreateSlider(settingsData.BrightnessData, videoSection);
            brightnessSlider.RegisterCallback<ChangeEvent<float>>(e => videoViewModel.SetBrightness(e.newValue));
            videoViewModel.Brightness.Subscribe(x => brightnessSlider.value = x);

            var bloomToggle = CreateToggle(settingsData.IsHighBloomQualityData, videoSection);
            bloomToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsHighBloomQuality(e.newValue));
            videoViewModel.IsHighBloomQuality.Subscribe(x => bloomToggle.value = x);

            var antiAliasingToggle = CreateToggle(settingsData.IsAntiAliasingEnabledData, videoSection);
            antiAliasingToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsAntiAliasingEnabled(e.newValue));
            videoViewModel.IsAntiAliasingEnabled.Subscribe(x => antiAliasingToggle.value = x);
        }

        private Slider CreateSlider(ISliderSettingData sliderSettingData, VisualElement parentSection)
        {
            var settingBar = _sliderSettingBarAsset.CloneTree();
            parentSection.Add(settingBar);

            var slider = settingBar.Q<Slider>();
            slider.lowValue = sliderSettingData.MinValue;
            slider.highValue = sliderSettingData.MaxValue;

            var label = settingBar.Q<Label>(className: _settingBarLabelClass);
            label.text = sliderSettingData.Label;

            return slider;
        }

        private Toggle CreateToggle(IToggleSettingData toggleSettingData, VisualElement parentSection)
        {
            var settingBar = _toggleSettingBarAsset.CloneTree();
            parentSection.Add(settingBar);

            var toggle = settingBar.Q<Toggle>();

            var label = settingBar.Q<Label>(className: _settingBarLabelClass);
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
                    ViewModel.CompleteClosing(); // TODO: replace it to the TransitionEndEvent
                // _contentElement.AddToClassList($"{_contentStyleName}--disabled");
                // _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundStyleName}--enabled");
                Debug.Log("Settings: closed");
            }
        }

        private void Close(ClickEvent clickEvent)
        {
            ViewModel.Close();
        }

        private void ApplyChanges(ClickEvent clickEvent)
        {
            ViewModel.ApplyChanges();
        }

        private void ResetSettings(ClickEvent clickEvent)
        {
            ViewModel.ResetCurrentSectionSettings();
        }
    }
}