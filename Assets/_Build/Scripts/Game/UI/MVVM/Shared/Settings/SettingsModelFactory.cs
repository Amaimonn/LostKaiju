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

        public SettingsModel Create()
        {
            return new SettingsModel(_gameStateProvider.Settings);
        }
    }
}