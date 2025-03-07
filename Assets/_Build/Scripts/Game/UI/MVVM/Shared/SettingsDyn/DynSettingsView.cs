using UnityEngine;
using UnityEngine.UIElements;
using R3;

using LostKaiju.Game.UI.MVVM.Gameplay;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    public class DynSettingsView : PopUpToolkitView<SettingsViewModel>
    {
        [SerializeField] private string _applyButtonName;
        [SerializeField] private string _resetButtonName;
        [SerializeField] private string _sectionsRootClass;
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
            foreach (var section in ViewModel.Sections.Values)
            {
                var data = section.Data;
                var sectionTab = new Tab()
                {
                    label = section.Data.Label
                };
                sectionTab.selected += _ => ViewModel.SelectSection(data.Id);
                sectionsRoot.Add(sectionTab);

                foreach (var settingBar in section.Data.SettingBarsData)
                {
                    switch (settingBar)
                    {
                        case ISliderSettingData sliderData:
                        {
                            var slider = CreateSlider(sliderData, sectionTab);
                            if (section.FloatMethods.TryGetValue(settingBar.NameId, out var method))
                            {
                                slider.RegisterCallback<ChangeEvent<float>>(e => method(e.newValue));
                                if (section.FloatSettings.TryGetValue(settingBar.NameId, out var floatSetting))
                                    floatSetting.Subscribe(x => slider.value = x);
                                else
                                    Debug.LogWarning($"No {settingBar.NameId} float setting key was found in ViewModel");
                            }
                            else
                            {
                                Debug.LogWarning($"No {settingBar.NameId} method key was found in ViewModel");
                            }
                            break;
                        }

                        case IToggleSettingData toggleData:
                        {
                            var toggle = CreateToggle(toggleData, sectionTab);
                            if (section.BoolMethods.TryGetValue(settingBar.NameId, out var method))
                            {
                                toggle.RegisterCallback<ChangeEvent<bool>>(e => method(e.newValue));
                                if (section.BoolSettings.TryGetValue(settingBar.NameId, out var boolSetting))
                                    boolSetting.Subscribe(x => toggle.value = x);
                                else
                                    Debug.LogWarning($"No {settingBar.NameId} bool setting key was found in ViewModel");
                            }
                            else
                            {
                                Debug.LogWarning($"No {settingBar.NameId} method key was found in ViewModel");
                            }
                            break;
                        }

                        default: 
                        {
                            Debug.LogWarning("Unknown setting bar data type");
                            break;
                        }
                    }
                }
            }
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