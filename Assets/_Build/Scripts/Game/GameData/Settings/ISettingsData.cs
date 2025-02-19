namespace LostKaiju.Game.GameData.Settings
{
    public interface ISettingsData
    {
        public string Label { get; }
        public ISettingsSectionData[] SectionsData { get; }
    }
}