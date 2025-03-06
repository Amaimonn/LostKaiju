using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "FullSettingsDataSO", menuName = "Scriptable Objects/Settings/FullSettingsDataSO")]
    public class FullSettingsDataSO : ScriptableObject, IFullSettingsData
    {
        [field: SerializeField] public string Label { get; private set; }
        [field: SerializeField] public string SoundSectionLabel { get; private set; }
        [field: SerializeField] public string VideoSectionLabel { get; private set; }
        [field: SerializeField] public string InputSectionLabel { get; private set; }
        public ISliderSettingData SoundVolumeData => _soundVolumeSliderDataSO;
        public ISliderSettingData SfxVolumeData => _sfxVolumeSliderDataSO;
        public ISliderSettingData BrightnessData => _brightnessSliderDataSO;
        public IToggleSettingData IsPostProcessingEnabledData => _IsPostProcessingEnabledToggleDataSO;
        public IToggleSettingData IsHighBloomQualityData => _IsHighBloomQualityToggleDataSO;
        public IToggleSettingData IsAntiAliasingEnabledData => _IsAntiAliasingEnabledToggleDataSO;

        [SerializeField] private SliderSettingDataSO _soundVolumeSliderDataSO;
        [SerializeField] private SliderSettingDataSO _sfxVolumeSliderDataSO;
        [SerializeField] private SliderSettingDataSO _brightnessSliderDataSO;
        [SerializeField] private ToggleSettingDataSO _IsPostProcessingEnabledToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsHighBloomQualityToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsAntiAliasingEnabledToggleDataSO;
    }
}