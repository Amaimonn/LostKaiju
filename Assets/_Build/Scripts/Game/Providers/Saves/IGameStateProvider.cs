using LostKaiju.Game.GameData;

namespace LostKaiju.Infrastructure.Providers.Inputs
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
    }
}
