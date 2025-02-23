using R3;
using System;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsViewModel : BaseScreenViewModel, IDisposable
    {
        public SettingsSectionViewModel CurrentSection => _currentSection;
        public readonly SoundSettingsViewModel SoundSettingsViewModel;
        public readonly VideoSettingsViewModel VideoSettingsViewModel;
        public readonly IFullSettingsData SettingsData;

        private readonly SettingsModel _model;
        private SettingsSectionViewModel _currentSection;

        public SettingsViewModel(SettingsModel model)
        {
            _model = model;
            SettingsData = model.SettingsData;
            SoundSettingsViewModel = new SoundSettingsViewModel(model);
            VideoSettingsViewModel = new VideoSettingsViewModel(model);
            _currentSection = SoundSettingsViewModel;
        }

        public override void StartClosing()
        {
            base.StartClosing();
            ResetUnappliedChanges();
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

        public void Dispose()
        {
            SoundSettingsViewModel.Dispose();
            VideoSettingsViewModel.Dispose();
        }
    }
}