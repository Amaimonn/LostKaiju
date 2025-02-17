using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsViewModel : IViewModel
    {
        public Observable<bool> OnOpenStateChanged => _isOpened;
        public Observable<Unit> OnClosingCompleted => _closingCompletedSignal;
        public SettingsSectionViewModel CurrentSection => _currentSection;

        private readonly SettingsModel _model;
        private readonly SoundSettingsViewModel _soundSettingsViewModel;
        private readonly VideoSettingsViewModel _videoSettingsViewModel;
        private readonly ReactiveProperty<bool> _isOpened = new(false);
        private readonly Subject<Unit> _closingCompletedSignal = new();
        private SettingsSectionViewModel _currentSection;

        public SettingsViewModel(SettingsModel model)
        {
            _model = model;
            _soundSettingsViewModel = new SoundSettingsViewModel(model);
            _videoSettingsViewModel = new VideoSettingsViewModel(model);
            _currentSection = _soundSettingsViewModel;
        }

        public void Open()
        {
            _isOpened.Value = true;
        }

        public void Close()
        {
            _isOpened.Value = false;
        }

        /// <summary>
        /// Complete closing when animation is finished. Used by View.
        /// </summary>
        public void CompleteClosing()
        {
            _closingCompletedSignal.OnNext(Unit.Default);
        }

        public void SetSoundSection()
        {
            _currentSection = _soundSettingsViewModel;
        }

        public void SetVideoSection()
        {
            _currentSection = _videoSettingsViewModel;
        }

        public void ApplyChanges()
        {
            _soundSettingsViewModel.ApplyChanges();
            _videoSettingsViewModel.ApplyChanges();
        }

        public void ResetCurrentSectionSettings()
        {
            _currentSection.ResetSettings();
        }
    }
}