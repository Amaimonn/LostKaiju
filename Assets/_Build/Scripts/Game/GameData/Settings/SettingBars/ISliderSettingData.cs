namespace LostKaiju.Game.GameData.Settings
{
    public interface ISliderSettingData : ISettingBarData
    {
        public float MinValue { get; }
        public float MaxValue { get; }
    }
}