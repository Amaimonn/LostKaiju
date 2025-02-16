using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SettingsViewModel : IViewModel
    {
        public Observable<bool> OnOpenStateChanged => _isOpened;
        public Observable<Unit> OnClosingCompleted => _closingCompletedSignal;

        private readonly SettingsModel _model;
        private readonly ReactiveProperty<bool> _isOpened = new(false);
        private readonly Subject<Unit> _closingCompletedSignal = new();
        private readonly ReactiveProperty<float> _soundVolume;
        private readonly ReactiveProperty<bool> _isSoundEnabled;
        private readonly ReactiveProperty<float> _sfxVolume;
        private readonly ReactiveProperty<bool> _isSfxEnabled;
        private readonly ReactiveProperty<float> _brightness;

        public SettingsViewModel(SettingsModel model)
        {
            _model = model;
            _soundVolume = new ReactiveProperty<float>(model.SoundVolume.Value);
            _isSoundEnabled = new ReactiveProperty<bool>(model.IsSoundEnabled.Value);
            _sfxVolume = new ReactiveProperty<float>(model.SfxVolume.Value);
            _isSfxEnabled = new ReactiveProperty<bool>(model.IsSfxEnabled.Value);
            _brightness = new ReactiveProperty<float>(model.Brightness.Value);
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


        public void ApplyChanges()
        {
            _model.SoundVolume.Value = _soundVolume.Value;
            _model.IsSoundEnabled.Value = _isSoundEnabled.Value;
            _model.SfxVolume.Value = _sfxVolume.Value;
            _model.IsSfxEnabled.Value = _isSfxEnabled.Value;
            _model.Brightness.Value = _brightness.Value;
        }

        public void ResetSettings()
        {
            _soundVolume.Value = _model.SoundVolume.Value;
            _isSoundEnabled.Value = _model.IsSoundEnabled.Value;
            _sfxVolume.Value = _model.SfxVolume.Value;
            _isSfxEnabled.Value = _model.IsSfxEnabled.Value;
        }

        public void ApplySoundChanges()
        {
            _model.SoundVolume.Value = _soundVolume.Value;
            _model.IsSoundEnabled.Value = _isSoundEnabled.Value;
            _model.SfxVolume.Value = _sfxVolume.Value;
            _model.IsSfxEnabled.Value = _isSfxEnabled.Value;
        }

        public void ApplyGraphicsChanges()
        {
            _model.Brightness.Value = _brightness.Value;
        }

        public void ApplyInputChanges()
        {

        }

        public void ResetSoundChanges()
        {
            _soundVolume.Value = _model.SoundVolume.Value;
            _isSoundEnabled.Value = _model.IsSoundEnabled.Value;
            _sfxVolume.Value = _model.SfxVolume.Value;
            _isSfxEnabled.Value = _model.IsSfxEnabled.Value;
        }

        public void ResetGraphicsChanges()
        {
            _brightness.Value = _model.Brightness.Value;
        }

        public void ResetInputChanges()
        {

        }
    }
}