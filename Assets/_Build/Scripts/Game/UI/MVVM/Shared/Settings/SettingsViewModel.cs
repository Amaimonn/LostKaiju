using System;

using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;
using R3;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsViewModel : ScreenViewModel, IDisposable
    {
        public SettingsSectionViewModel CurrentSection => _currentSection;
        public Observable<IFullSettingsData> SettingsData => _settingsData;
        public bool IsApplyPopUpOpened => false; // TODO: popup
        public readonly SoundSettingsViewModel SoundSettingsViewModel;
        public readonly VideoSettingsViewModel VideoSettingsViewModel;

        private readonly IGameStateProvider _gameStateProvider;
        private SettingsSectionViewModel _currentSection;
        private readonly ReactiveProperty<IFullSettingsData> _settingsData = new();

        public SettingsViewModel(SettingsModel model, IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
            SoundSettingsViewModel = new SoundSettingsViewModel(model);
            VideoSettingsViewModel = new VideoSettingsViewModel(model);
            _currentSection = SoundSettingsViewModel;
        }

        public void BindData(IFullSettingsData settingsData)
        {
            _settingsData.Value = settingsData;
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