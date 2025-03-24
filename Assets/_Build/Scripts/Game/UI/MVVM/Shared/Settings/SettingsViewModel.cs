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
        public Observable<bool> IsAnyChanges => _isAnyChanges;
        public readonly SoundSettingsViewModel SoundSettingsViewModel;
        public readonly VideoSettingsViewModel VideoSettingsViewModel;

        private readonly IGameStateProvider _gameStateProvider;
        private SettingsSectionViewModel _currentSection;
        private readonly ReactiveProperty<IFullSettingsData> _settingsData = new();
        private readonly ReadOnlyReactiveProperty<bool> _isAnyChanges;

        public SettingsViewModel(SettingsModel model, IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
            SoundSettingsViewModel = new SoundSettingsViewModel(model);
            VideoSettingsViewModel = new VideoSettingsViewModel(model);
            _currentSection = SoundSettingsViewModel;
            _isAnyChanges = Observable.CombineLatest(SoundSettingsViewModel.IsAnyChanges, 
                VideoSettingsViewModel.IsAnyChanges, (soundChanges, videoChanges) => soundChanges || videoChanges)
                .ToReadOnlyReactiveProperty();
        }

        public void BindData(IFullSettingsData settingsData)
        {
            _settingsData.Value = settingsData;
        }

        public override void StartClosing()
        {
            CancelUnappliedChanges();
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
            base.StartClosing();
        }

        public void CancelUnappliedChanges()
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