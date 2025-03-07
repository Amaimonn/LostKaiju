using LostKaiju.Game.GameData.SettingsDyn;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    public class VideoSettingsViewModel : SettingsSectionViewModel
    {
        public VideoSettingsViewModel(SettingsModel model, ISettingsSectionData data) : base(model, data)
        {
            var floatSettingsToInit = new string[]
            {
                SettingNames.Brightness
            };

            var boolSettingsToInit = new string[]
            {
                SettingNames.IsPostProcessingEnabled,
                SettingNames.IsHighBloomQuality,
                SettingNames.IsAntiAliasingEnabled
            };

            InitSettings(floatNames: floatSettingsToInit, boolNames: boolSettingsToInit);
        }
    }
}