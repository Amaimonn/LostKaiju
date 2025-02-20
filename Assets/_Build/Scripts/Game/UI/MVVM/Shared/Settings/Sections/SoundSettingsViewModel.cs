using System.Linq;
using R3;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SoundSettingsViewModel : SettingsSectionViewModel
    {
        public Observable<float> SoundVolume => _soundVolume;
        public Observable<bool> IsSoundEnabled => _isSoundEnabled;
        public Observable<float> SfxVolume => _sfxVolume;
        public Observable<bool> IsSfxEnabled => _isSfxEnabled;

        private readonly ReactiveProperty<float> _soundVolume;
        private readonly ReactiveProperty<bool> _isSoundEnabled;
        private readonly ReactiveProperty<float> _sfxVolume;
        private readonly ReactiveProperty<bool> _isSfxEnabled;
        private float _soundVolumeCached;
        private bool _isSoundEnabledCached;
        private float _sfxVolumeCached;
        private bool _isSfxEnabledCached;


        public SoundSettingsViewModel(SettingsModel model) : base(model)
        {
            SetCachedSettings();
            
            _soundVolume = new ReactiveProperty<float>(_soundVolumeCached);
            model.SoundVolume.Skip(1).Subscribe(x => _soundVolume.Value = x).AddTo(_disposables);

            _isSoundEnabled = new ReactiveProperty<bool>(_isSoundEnabledCached);
            model.IsSoundEnabled.Skip(1).Subscribe(x => _isSoundEnabled.Value = x).AddTo(_disposables);

            _sfxVolume = new ReactiveProperty<float>(_sfxVolumeCached);
            model.SfxVolume.Skip(1).Subscribe(x => _sfxVolume.Value = x).AddTo(_disposables);

            _isSfxEnabled = new ReactiveProperty<bool>(_isSfxEnabledCached);
            model.IsSfxEnabled.Skip(1).Subscribe(x => _isSfxEnabled.Value = x).AddTo(_disposables);

            IsAnyChanges = Observable.CombineLatest(
                    _soundVolume.Select(x => x != _soundVolumeCached),
                    _isSoundEnabled.Select(x => x != _isSoundEnabledCached),
                    _sfxVolume.Select(x => x != _sfxVolumeCached),
                    _isSfxEnabled.Select(x => x != _isSfxEnabledCached)
                ).Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }


        public override void ApplyChanges()
        {
            // set additional data to model if needed 
            SetCachedSettings();
        }

        public override void ResetSettings()
        {
            _model.SoundVolume.Value = _soundVolumeCached;
            _model.IsSoundEnabled.Value = _isSoundEnabledCached;
            _model.SfxVolume.Value = _sfxVolumeCached;
            _model.IsSfxEnabled.Value = _isSfxEnabledCached;
        }

        public void SetSoundVolume(float volume)
        {
            _model.SoundVolume.Value = volume;
            _isSoundEnabled.Value = volume > 0;
        }

        public void SetSfxVolume(float volume)
        {
            _model.SfxVolume.Value = volume;
            _isSfxEnabled.Value = volume > 0;
        }

        protected override void SetCachedSettings()
        {
            _soundVolumeCached = _model.SoundVolume.Value;
            _isSoundEnabledCached = _model.IsSoundEnabled.Value;
            _sfxVolumeCached = _model.SfxVolume.Value;
            _isSfxEnabledCached = _model.IsSfxEnabled.Value;
        }
    }
}