using LostKaiju.GameData;

namespace LostKaiju.Architecture.Providers
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
    }
}
