namespace LostKaiju.Game.Player.Data.Configs
{
    public interface IPlayerData
    {
        public PlayerControlsData PlayerControlsData { get; }
        public PlayerDefenceData PlayerDefenceData { get; }
    }
}