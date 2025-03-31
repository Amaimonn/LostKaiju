using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "FullSettingsDataSO", menuName = "Scriptable Objects/Settings/FullSettingsDataSO")]
    public class FullSettingsDataSO : ScriptableObject, IFullSettingsData
    {
        [field: SerializeField] public string SoundSectionLabel { get; private set; }
        [field: SerializeField] public string VideoSectionLabel { get; private set; }
        [field: SerializeField] public string InputSectionLabel { get; private set; }
        [field: SerializeField] public string LanguageSectionLabel { get; private set; }

        public ISliderSettingData MusicVolumeData => _musicVolumeSliderDataSO;
        public ISliderSettingData SfxVolumeData => _sfxVolumeSliderDataSO;
        public ISliderSettingData BrightnessData => _brightnessSliderDataSO;
        public IToggleSettingData IsPostProcessingEnabledData => _IsPostProcessingEnabledToggleDataSO;
        public IToggleSettingData IsBloomEnabledData => _IsBloomEnabledToggleDataSO;
        public IToggleSettingData IsFilmGrainEnabledData => _IsFilmGrainEnabledToggleDataSO;
        public IToggleSettingData IsAntiAliasingEnabledData => _IsAntiAliasingEnabledToggleDataSO;
        public IArrowsSettingData LanguageData => _languageDataSO;

        [SerializeField] private SliderSettingDataSO _musicVolumeSliderDataSO;
        [SerializeField] private SliderSettingDataSO _sfxVolumeSliderDataSO;
        [SerializeField] private SliderSettingDataSO _brightnessSliderDataSO;
        [SerializeField] private ToggleSettingDataSO _IsPostProcessingEnabledToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsBloomEnabledToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsFilmGrainEnabledToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsAntiAliasingEnabledToggleDataSO;
        [SerializeField] private ArrowsSettingDataSO _languageDataSO;
    }
}