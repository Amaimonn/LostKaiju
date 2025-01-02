using LostKaiju.Gameplay.GameData;

namespace LostKaiju.Infrastructure.Providers.Inputs
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
    }
}
