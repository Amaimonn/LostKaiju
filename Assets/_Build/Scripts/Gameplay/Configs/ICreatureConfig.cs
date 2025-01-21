using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Player.Data.Configs;

namespace LostKaiju.Game.Configs
{
    public interface IPlayerConfig
    {
        public CreatureBinder CreatureBinder { get; }
        public IPlayerData PlayerData { get; }
    }
}
