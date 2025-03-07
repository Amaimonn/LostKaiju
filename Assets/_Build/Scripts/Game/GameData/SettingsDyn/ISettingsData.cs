namespace LostKaiju.Game.GameData.SettingsDyn
{
    public interface ISettingsData
    {
        public string Label { get; }
        public ISettingsSectionData[] SectionsData { get; }
    }
}