using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.GameData.SettingsDyn
{
    public interface ISettingsSectionData
    {
        public string Id { get; }
        public string Label { get; }
        public ISettingBarData[] SettingBarsData { get; }
    }
}