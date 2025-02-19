namespace LostKaiju.Game.GameData.Settings
{
    public interface ISettingsSectionData
    {
        public string Id { get; }
        public string Label { get; }
        public ISettingBarData[] SettingBarsData { get; }
    }
}