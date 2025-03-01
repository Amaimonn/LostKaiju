using LostKaiju.Game.World.Creatures.Views;

namespace LostKaiju.Game.World.Player.Data.Configs
{
    public interface IPlayerConfig
    {
        public CreatureBinder CreatureBinder { get; }
        public IPlayerData PlayerData { get; }
    }
}
