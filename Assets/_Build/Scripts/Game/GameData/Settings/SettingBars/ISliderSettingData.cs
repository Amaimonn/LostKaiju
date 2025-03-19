namespace LostKaiju.Game.GameData.Settings
{
    public interface ISliderSettingData : ISettingBarData
    {
        public int MinValue { get; }
        public int MaxValue { get; }
    }
}