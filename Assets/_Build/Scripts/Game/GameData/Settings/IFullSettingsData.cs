namespace LostKaiju.Game.GameData.Settings
{
    public interface IFullSettingsData
    {
        public string SoundSectionLabel { get; }
        public string VideoSectionLabel { get; }
        public string InputSectionLabel { get; }
        public string LanguageSectionLabel { get; }
        public ISliderSettingData MusicVolumeData { get; }
        public ISliderSettingData SfxVolumeData { get; }
        public ISliderSettingData BrightnessData { get; }
        public IToggleSettingData IsPostProcessingEnabledData { get; }
        public IToggleSettingData IsBloomEnabledData { get; }
        public IToggleSettingData IsFilmGrainEnabledData { get; }
        public IToggleSettingData IsAntiAliasingEnabledData { get; }
        public IArrowsSettingData LanguageData { get; }
    }
}