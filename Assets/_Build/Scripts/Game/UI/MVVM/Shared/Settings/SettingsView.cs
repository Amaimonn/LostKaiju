using System;
using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.UI.Extentions;
using LostKaiju.Game.Constants;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsView : PopUpToolkitView<SettingsViewModel>
    {
        [Header("UIElements")]
        [SerializeField] private string _applyButtonName;
        [SerializeField] private string _cancelChangesButtonName;
        [SerializeField] private string _sectionsRootClass;

        [Header("Sections"), Space(4)]
        [SerializeField] private VisualTreeAsset _sectionScrollViewAsset;
        
        [Header("Controls"), Space(4)]
        [SerializeField] private string _settingBarLabelClass;
        [SerializeField] private VisualTreeAsset _sliderSettingBarAsset;
        [SerializeField] private VisualTreeAsset _toggleSettingBarAsset;

        private Button _applyButton;
        private Button _cancelChangesButton;
        // private bool _isClosing = false;

        protected override void OnAwake()
        {
            base.OnAwake();
            _applyButton = Root.Q<Button>(name: _applyButtonName);
            _cancelChangesButton = Root.Q<Button>(name: _cancelChangesButtonName);
        }

        protected override void OnBind(SettingsViewModel viewModel)
        {
            base.OnBind(viewModel);
            InitSections();
            _applyButton.RegisterCallback<ClickEvent>(ApplyChanges);
            _cancelChangesButton.RegisterCallback<ClickEvent>(CancelChanges);
            ViewModel.IsAnyChanges.Subscribe(x => 
            {
                _applyButton.SetEnabled(x);
                _cancelChangesButton.SetEnabled(x);
            });
        }

        private void InitSections()
        {
            var sectionsRoot = Root.Q<VisualElement>(className: _sectionsRootClass);
            sectionsRoot.Clear();
            ViewModel.SettingsData.Where(x => x != null).Subscribe(data =>
            {
                InitSoundSection(sectionsRoot, data);
                InitVideoSection(sectionsRoot, data);
            }).AddTo(_disposables);
        }
        
        private void InitSoundSection(VisualElement sectionsRoot, IFullSettingsData settingsData)
        {
            var soundViewModel = ViewModel.SoundSettingsViewModel;
            
            var soundSection = new Tab();
            soundSection.LocalizeLabel(Tables.SETTINGS, settingsData.SoundSectionLabel);
            soundSection.selected += _ => ViewModel.SelectSoundSection();
            sectionsRoot.Add(soundSection);

            var scrollView = CreateScrollView(soundSection);

            var soundVolumeSlider = CreateSliderInt(settingsData.MusicVolumeData, scrollView);
            BindSliderInt(soundVolumeSlider, soundViewModel.SetMusicVolume, soundViewModel.MusicVolume);

            var sfxVolumeSlider = CreateSliderInt(settingsData.SfxVolumeData, scrollView);
            BindSliderInt(sfxVolumeSlider, soundViewModel.SetSfxVolume, soundViewModel.SfxVolume);
        }

        private void InitVideoSection(VisualElement sectionsRoot, IFullSettingsData settingsData)
        {
            var videoViewModel = ViewModel.VideoSettingsViewModel;

            var videoSection = new Tab();
            videoSection.LocalizeLabel(Tables.SETTINGS, settingsData.VideoSectionLabel);
            videoSection.selected += _ => ViewModel.SelectVideoSection();
            sectionsRoot.Add(videoSection);

            var scrollView = CreateScrollView(videoSection);

            var brightnessSlider = CreateSliderInt(settingsData.BrightnessData, scrollView);
            BindSliderInt(brightnessSlider, videoViewModel.SetBrightness, videoViewModel.Brightness);

            var postProcessingToggle = CreateToggle(settingsData.IsPostProcessingEnabledData, scrollView);
            BindToggle(postProcessingToggle, videoViewModel.SetIsPostProcessingEnabled, videoViewModel.IsPostProcessingEnabled);

            var bloomToggle = CreateToggle(settingsData.IsBloomEnabledData, scrollView);
            BindToggle(bloomToggle, videoViewModel.SetIsBloomEnabled, videoViewModel.IsHighBloomQuality);

            var antiAliasingToggle = CreateToggle(settingsData.IsAntiAliasingEnabledData, scrollView);
            BindToggle(antiAliasingToggle, videoViewModel.SetIsAntiAliasingEnabled, videoViewModel.IsAntiAliasingEnabled);
        }

        private ScrollView CreateScrollView(VisualElement parentSection)
        {
            var scrollViewConteiner = _sectionScrollViewAsset.CloneTree();//new ScrollView();
            var scrollView = scrollViewConteiner.Q<ScrollView>();
            parentSection.Add(scrollView);

            return scrollView;
        }

        private SliderInt CreateSliderInt(ISliderSettingData sliderSettingData, VisualElement parentSection)
        {
            var settingBar = _sliderSettingBarAsset.CloneTree();
            parentSection.Add(settingBar);

            var slider = settingBar.Q<SliderInt>();
            slider.lowValue = sliderSettingData.MinValue;
            slider.highValue = sliderSettingData.MaxValue;

            var label = settingBar.Q<Label>(className: _settingBarLabelClass);
            label.LocalizeText(Tables.SETTINGS, sliderSettingData.Label);

            return slider;
        }

        private void BindSliderInt(SliderInt slider, Action<int> method, Observable<int> observable)
        {
            slider.RegisterCallback<ChangeEvent<int>>(e => method(e.newValue));
            observable.Subscribe(x => slider.value = x);
            //sliderSettingData.PageSize
        }

        private Toggle CreateToggle(IToggleSettingData toggleSettingData, VisualElement parentSection)
        {
            var settingBar = _toggleSettingBarAsset.CloneTree();
            parentSection.Add(settingBar);

            var toggle = settingBar.Q<Toggle>();

            var label = settingBar.Q<Label>(className: _settingBarLabelClass);
            label.LocalizeText(Tables.SETTINGS, toggleSettingData.Label);

            return toggle;
        }

        private void BindToggle(Toggle toggle, Action<bool> method, Observable<bool> observable)
        {
            toggle.RegisterCallback<ChangeEvent<bool>>(e => method(e.newValue));
            observable.Subscribe(x => toggle.value = x);
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

        private void CancelChanges(ClickEvent clickEvent)
        {
            // ViewModel.ResetCurrentSectionSettings();
            ViewModel.CancelUnappliedChanges();
        }
    }
}