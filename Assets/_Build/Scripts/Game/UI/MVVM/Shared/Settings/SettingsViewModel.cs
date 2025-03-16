using System;

using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsViewModel : ScreenViewModel, IDisposable
    {
        public SettingsSectionViewModel CurrentSection => _currentSection;
        public bool IsApplyPopUpOpened => false; // TODO: popup
        public readonly SoundSettingsViewModel SoundSettingsViewModel;
        public readonly VideoSettingsViewModel VideoSettingsViewModel;
        public readonly IFullSettingsData SettingsData;

        private readonly IGameStateProvider _gameStateProvider;
        private SettingsSectionViewModel _currentSection;

        public SettingsViewModel(SettingsModel model, IFullSettingsData settingsData,
            IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
            SettingsData = settingsData;
            SoundSettingsViewModel = new SoundSettingsViewModel(model);
            VideoSettingsViewModel = new VideoSettingsViewModel(model);
            _currentSection = SoundSettingsViewModel;
        }

        public override void StartClosing()
        {
            ResetUnappliedChanges();
            base.StartClosing();
        }

        public void SelectSoundSection()
        {
            _currentSection = SoundSettingsViewModel;
        }

        public void SelectVideoSection()
        {
            _currentSection = VideoSettingsViewModel;
        }

        public void ApplyChanges()
        {
            SoundSettingsViewModel.ApplyChanges();
            VideoSettingsViewModel.ApplyChanges();
            SaveSettings();
        }

        public void ResetUnappliedChanges()
        {
            SoundSettingsViewModel.ResetSettings();
            VideoSettingsViewModel.ResetSettings();
        }

        public void ResetCurrentSectionSettings()
        {
            _currentSection.ResetSettings();
        }

        private void SaveSettings()
        {
            _gameStateProvider.SaveSettingsAsync();
        }

        public void Dispose()
        {
            SoundSettingsViewModel.Dispose();
            VideoSettingsViewModel.Dispose();
        }
    }
}