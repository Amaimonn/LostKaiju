using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Gameplay.Player.Data.Configs;

namespace LostKaiju.Gameplay.Configs
{
    public interface IPlayerConfig
    {
        public CreatureBinder CreatureBinder { get; }
        public IPlayerData PlayerData { get; }
    }
}
