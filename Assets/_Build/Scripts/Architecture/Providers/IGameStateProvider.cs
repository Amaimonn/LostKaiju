using Assets._Build.Scripts.GameData;

namespace Assets._Build.Scripts.Architecture.Providers
{
    public interface IGameStateProvider
    {
        public SettingsModel Settings { get; }
    }
}
