using LostKaiju.Game.GameData.SettingsDyn;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    public class SoundSettingsViewModel : SettingsSectionViewModel
    {
        public SoundSettingsViewModel(SettingsModel model, ISettingsSectionData data) : base(model, data)
        {
            var floatSettingsToInit = new string[]
            {
                SettingNames.SoundVolume,
                SettingNames.SfxVolume
                // SettingNames.Brightness,
                // SettingNames.IsPostProcessingEnabled,
                // SettingNames.IsHighBloomQuality,
                // SettingNames.IsAntiAliasingEnabled,
            };

            var boolSettingsToInit = new string[]
            {
                SettingNames.IsSoundEnabled,
                SettingNames.IsSfxEnabled,
            };

            InitSettings(floatNames: floatSettingsToInit, boolNames: boolSettingsToInit);
        }
    }
}