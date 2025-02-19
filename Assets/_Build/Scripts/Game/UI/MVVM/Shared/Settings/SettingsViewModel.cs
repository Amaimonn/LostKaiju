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
        public readonly SoundSettingsViewModel SoundSettingsViewModel;
        public readonly VideoSettingsViewModel VideoSettingsViewModel;
        public readonly IFullSettingsData SettingsData;

        private readonly SettingsModel _model;
        private readonly ReactiveProperty<bool> _isOpened = new(false);
        private readonly Subject<Unit> _closingCompletedSignal = new();
        private SettingsSectionViewModel _currentSection;

        public SettingsViewModel(SettingsModel model)
        {
            _model = model;
            SettingsData = model.SettingsData;
            SoundSettingsViewModel = new SoundSettingsViewModel(model);
            VideoSettingsViewModel = new VideoSettingsViewModel(model);
            _currentSection = SoundSettingsViewModel;
        }

        public void Open()
        {
            _isOpened.Value = true;
        }

        public void Close()
        {
            _isOpened.Value = false;
            ResetUnappliedChanges();
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
            _currentSection = SoundSettingsViewModel;
        }

        public void SetVideoSection()
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
    }
}