namespace LostKaiju.Game.GameData.Settings
{
    public interface ISettingBarData
    {
        public string Label { get; }
        public string NameId { get; }
        public bool SetAfterApply { get; }
    }
}