using LostKaiju.Game.Creatures.Views;

namespace LostKaiju.Game.Player.Data.Configs
{
    public interface IPlayerConfig
    {
        public CreatureBinder CreatureBinder { get; }
        public IPlayerData PlayerData { get; }
    }
}
