using LostKaiju.GameData;

namespace LostKaiju.Architecture.Providers.Inputs
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
    }
}
