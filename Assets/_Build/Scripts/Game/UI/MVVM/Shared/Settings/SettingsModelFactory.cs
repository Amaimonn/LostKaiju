using UnityEngine;
using VContainer;

using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsModelFactory
    {
        [Inject]
        private readonly IGameStateProvider _gameStateProvider;
        private readonly string _settingsDataPath;

        public SettingsModelFactory(string settingsDataPath)
        {
            _settingsDataPath = settingsDataPath;
        }

        public SettingsModel Create()
        {
            var settingsData = Resources.Load<FullSettingsDataSO>(_settingsDataPath);
            return new SettingsModel(_gameStateProvider.Settings, settingsData);
        }
    }
}