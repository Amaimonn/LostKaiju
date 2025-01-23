using LostKaiju.Game.GameData;

namespace LostKaiju.Game.Providers.GameState
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
    }
}
