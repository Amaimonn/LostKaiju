using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.UI.MVVM.Gameplay;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsView : PopUpToolkitView<SettingsViewModel>
    {
        [SerializeField] private string _applyButtonName;
        [SerializeField] private string _resetButtonName;
        [SerializeField] private string _sectionsRootClass;
        [SerializeField] private string _scrollViewClass;
        [SerializeField] private string _settingBarLabelClass;
        [SerializeField] private VisualTreeAsset _sliderSettingBarAsset;
        [SerializeField] private VisualTreeAsset _toggleSettingBarAsset;

        private Button _applyButton;
        private Button _resetButton;
        // private bool _isClosing = false;

        protected override void OnAwake()
        {
            base.OnAwake();
            _applyButton = Root.Q<Button>(name: _applyButtonName);
            _resetButton = Root.Q<Button>(name: _resetButtonName);
        }

        protected override void OnBind(SettingsViewModel viewModel)
        {
            base.OnBind(viewModel);
            InitSections();
            _applyButton.RegisterCallback<ClickEvent>(ApplyChanges);
            _resetButton.RegisterCallback<ClickEvent>(ResetSettings);
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

            var scrollView = CreateScrollView(soundSection);

            var soundVolumeSlider = CreateSlider(settingsData.SoundVolumeData, scrollView);
            soundVolumeSlider.RegisterCallback<ChangeEvent<float>>(e => soundViewModel.SetSoundVolume(e.newValue));
            ViewModel.SoundSettingsViewModel.SoundVolume.Subscribe(x => soundVolumeSlider.value = x);

            var sfxVolumeSlider = CreateSlider(settingsData.SfxVolumeData, scrollView);
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

            var scrollView = CreateScrollView(videoSection);

            var brightnessSlider = CreateSlider(settingsData.BrightnessData, scrollView);
            brightnessSlider.RegisterCallback<ChangeEvent<float>>(e => videoViewModel.SetBrightness(e.newValue));
            videoViewModel.Brightness.Subscribe(x => brightnessSlider.value = x);

            var postProcessingToggle = CreateToggle(settingsData.IsPostProcessingEnabledData, scrollView);
            postProcessingToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsPostProcessingEnabled(e.newValue));
            videoViewModel.IsPostProcessingEnabled.Subscribe(x => postProcessingToggle.value = x);

            var bloomToggle = CreateToggle(settingsData.IsHighBloomQualityData, scrollView);
            bloomToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsHighBloomQuality(e.newValue));
            videoViewModel.IsHighBloomQuality.Subscribe(x => bloomToggle.value = x);

            var antiAliasingToggle = CreateToggle(settingsData.IsAntiAliasingEnabledData, scrollView);
            antiAliasingToggle.RegisterCallback<ChangeEvent<bool>>(e => videoViewModel.SetIsAntiAliasingEnabled(e.newValue));
            videoViewModel.IsAntiAliasingEnabled.Subscribe(x => antiAliasingToggle.value = x);
        }

        private ScrollView CreateScrollView(VisualElement parentSection)
        {
            var scrollView = new ScrollView();
            scrollView.AddToClassList(_scrollViewClass);
            parentSection.Add(scrollView);

            return scrollView;
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
        
#region PopUpToolkitView
        protected override void OnOpening()
        {
            Debug.Log("Settings: opened");
            // StartCoroutine(OpenAnimation());

            // static IEnumerator OpenAnimation()
            // {
            //     yield return null;
            //     // _contentElement.RemoveFromClassList($"{_contentStyleName}--disabled");
            //     // _panelWhiteBackground.AddToClassList($"{_panelWhiteBackgroundStyleName}--enabled");
                
            // }
        }

        protected override void OnClosing()
        {
            // _isClosing = true;
            // Root.SetEnabled(false);
            // if (_isClosing)
                 // TODO: replace it to the TransitionEndEvent
            // _contentElement.AddToClassList($"{_contentStyleName}--disabled");
            // _panelWhiteBackground.RemoveFromClassList($"{_panelWhiteBackgroundStyleName}--enabled");
            Debug.Log("Settings: closed");
            base.OnClosing();
        }
#endregion

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