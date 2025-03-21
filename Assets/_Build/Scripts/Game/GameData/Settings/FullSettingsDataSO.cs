using UnityEngine;

namespace LostKaiju.Game.GameData.Settings
{
    [CreateAssetMenu(fileName = "FullSettingsDataSO", menuName = "Scriptable Objects/Settings/FullSettingsDataSO")]
    public class FullSettingsDataSO : ScriptableObject, IFullSettingsData
    {
        [field: SerializeField] public string SoundSectionLabel { get; private set; }
        [field: SerializeField] public string VideoSectionLabel { get; private set; }
        [field: SerializeField] public string InputSectionLabel { get; private set; }
        public ISliderSettingData MusicVolumeData => _musicVolumeSliderDataSO;
        public ISliderSettingData SfxVolumeData => _sfxVolumeSliderDataSO;
        public ISliderSettingData BrightnessData => _brightnessSliderDataSO;
        public IToggleSettingData IsPostProcessingEnabledData => _IsPostProcessingEnabledToggleDataSO;
        public IToggleSettingData IsBloomEnabledData => _IsBloomEnabledToggleDataSO;
        public IToggleSettingData IsAntiAliasingEnabledData => _IsAntiAliasingEnabledToggleDataSO;

        [SerializeField] private SliderSettingDataSO _musicVolumeSliderDataSO;
        [SerializeField] private SliderSettingDataSO _sfxVolumeSliderDataSO;
        [SerializeField] private SliderSettingDataSO _brightnessSliderDataSO;
        [SerializeField] private ToggleSettingDataSO _IsPostProcessingEnabledToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsBloomEnabledToggleDataSO;
        [SerializeField] private ToggleSettingDataSO _IsAntiAliasingEnabledToggleDataSO;
    }
}