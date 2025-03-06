namespace LostKaiju.Game.GameData.Settings
{
    public interface IFullSettingsData
    {
        public string Label { get; }
        public string SoundSectionLabel { get; }
        public string VideoSectionLabel { get; }
        public string InputSectionLabel { get; }
        public ISliderSettingData SoundVolumeData { get; }
        public ISliderSettingData SfxVolumeData { get; }
        public ISliderSettingData BrightnessData { get; }
        public IToggleSettingData IsPostProcessingEnabledData { get; }
        public IToggleSettingData IsHighBloomQualityData { get; }
        public IToggleSettingData IsAntiAliasingEnabledData { get; }
    }
}