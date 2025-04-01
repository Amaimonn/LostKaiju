using System.Linq;
using R3;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class SoundSettingsViewModel : SettingsSectionViewModel
    {
        public Observable<int> MusicVolume => _musicVolume;
        public Observable<bool> IsSoundEnabled => _isSoundEnabled;
        public Observable<int> SfxVolume => _sfxVolume;
        public Observable<bool> IsSfxEnabled => _isSfxEnabled;

        private readonly ReactiveProperty<int> _musicVolume;
        private readonly ReactiveProperty<bool> _isSoundEnabled;
        private readonly ReactiveProperty<int> _sfxVolume;
        private readonly ReactiveProperty<bool> _isSfxEnabled;
        private int _musicVolumeCached;
        private bool _isSoundEnabledCached;
        private int _sfxVolumeCached;
        private bool _isSfxEnabledCached;


        public SoundSettingsViewModel(SettingsModel model) : base(model)
        {
            CacheSettings();
            
            _musicVolume = new ReactiveProperty<int>(_musicVolumeCached);
            model.MusicVolume.Skip(1).Subscribe(x => _musicVolume.Value = x).AddTo(_disposables);

            _isSoundEnabled = new ReactiveProperty<bool>(_isSoundEnabledCached);
            model.IsMusicEnabled.Skip(1).Subscribe(x => _isSoundEnabled.Value = x).AddTo(_disposables);

            _sfxVolume = new ReactiveProperty<int>(_sfxVolumeCached);
            model.SfxVolume.Skip(1).Subscribe(x => _sfxVolume.Value = x).AddTo(_disposables);

            _isSfxEnabled = new ReactiveProperty<bool>(_isSfxEnabledCached);
            model.IsSfxEnabled.Skip(1).Subscribe(x => _isSfxEnabled.Value = x).AddTo(_disposables);

            IsAnyChanges = Observable.CombineLatest(
                    _musicVolume.Select(x => x != _musicVolumeCached),
                    _isSoundEnabled.Select(x => x != _isSoundEnabledCached),
                    _sfxVolume.Select(x => x != _sfxVolumeCached),
                    _isSfxEnabled.Select(x => x != _isSfxEnabledCached)
                ).Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }


        public override void ApplyChanges()
        {
            // set additional data to model if needed 
            CacheSettings();
        }

        public override void CancelChanges()
        {
            _model.MusicVolume.Value = _musicVolumeCached;
            _model.IsMusicEnabled.Value = _isSoundEnabledCached;
            _model.SfxVolume.Value = _sfxVolumeCached;
            _model.IsSfxEnabled.Value = _isSfxEnabledCached;
        }

        public void SetMusicVolume(int volume)
        {
            _model.MusicVolume.Value = volume;
            _isSoundEnabled.Value = volume > 0;
        }

        public void SetSfxVolume(int volume)
        {
            _model.SfxVolume.Value = volume;
            _isSfxEnabled.Value = volume > 0;
        }

        protected override void CacheSettings()
        {
            _musicVolumeCached = _model.MusicVolume.Value;
            _isSoundEnabledCached = _model.IsMusicEnabled.Value;
            _sfxVolumeCached = _model.SfxVolume.Value;
            _isSfxEnabledCached = _model.IsSfxEnabled.Value;
        }
    }
}