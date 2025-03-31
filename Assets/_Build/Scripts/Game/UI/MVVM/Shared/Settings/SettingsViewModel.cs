using System;
using System.Linq;
using R3;

using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.Providers.GameState;

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
        public readonly LanguageSectionViewModel LanguageSettingsViewModel;

        private readonly IGameStateProvider _gameStateProvider;
        private SettingsSectionViewModel _currentSection;
        private readonly ReactiveProperty<IFullSettingsData> _settingsData = new();
        private readonly ReadOnlyReactiveProperty<bool> _isAnyChanges;

        public SettingsViewModel(SettingsModel model, IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
            SoundSettingsViewModel = new SoundSettingsViewModel(model);
            VideoSettingsViewModel = new VideoSettingsViewModel(model);
            LanguageSettingsViewModel = new LanguageSectionViewModel(model);
            _currentSection = SoundSettingsViewModel;
            
            _isAnyChanges = Observable.CombineLatest(
                SoundSettingsViewModel.IsAnyChanges, 
                VideoSettingsViewModel.IsAnyChanges, 
                LanguageSettingsViewModel.IsAnyChanges)
                .Select(x => x.Any(t => t == true))
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

        public void SelectLanguageSection()
        {
            _currentSection = LanguageSettingsViewModel;
        }

        public void ApplyChanges()
        {
            SoundSettingsViewModel.ApplyChanges();
            VideoSettingsViewModel.ApplyChanges();
            LanguageSettingsViewModel.ApplyChanges();
            SaveSettings();
            base.StartClosing();
        }

        public void CancelUnappliedChanges()
        {
            SoundSettingsViewModel.CancelChanges();
            VideoSettingsViewModel.CancelChanges();
        }

        public void ResetCurrentSectionSettings()
        {
            _currentSection.CancelChanges();
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