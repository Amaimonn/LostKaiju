using System.Linq;
using R3;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class VideoSettingsViewModel : SettingsSectionViewModel
    {
        public Observable<float> Brightness => _brightness;
        public Observable<bool> IsHighBloomQuality => _isHighBloomQuality;
        public Observable<bool> IsAntiAliasingEnabled => _isAntiAliasingEnabled;

        private readonly ReactiveProperty<float> _brightness;
        private readonly ReactiveProperty<bool> _isHighBloomQuality;
        private readonly ReactiveProperty<bool> _isAntiAliasingEnabled;
        private float _brightnessCached;
        private bool _isHighBloomQualityCached;
        private bool _isAntiAliasingEnabledCached;

        public VideoSettingsViewModel(SettingsModel model) : base(model)
        {
            SetCachedSettings();

            _brightness = new ReactiveProperty<float>(model.Brightness.Value);
            model.Brightness.Skip(1).Subscribe(x => _brightness.Value = x).AddTo(_disposables);

            _isHighBloomQuality = new ReactiveProperty<bool>(model.IsHighBloomQuality.Value);
            model.IsHighBloomQuality.Skip(1).Subscribe(x => _isHighBloomQuality.Value = x).AddTo(_disposables);

            _isAntiAliasingEnabled = new ReactiveProperty<bool>(model.IsAntiAliasingEnabled.Value);
            model.IsAntiAliasingEnabled.Skip(1).Subscribe(x => _isAntiAliasingEnabled.Value = x).AddTo(_disposables);

            IsAnyChanges = Observable.CombineLatest(
                    _brightness.Select(x => x != _brightnessCached),
                    _isHighBloomQuality.Select(x => x != _isHighBloomQualityCached),
                    _isAntiAliasingEnabled.Select(x => x != _isAntiAliasingEnabledCached)
                ).Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }


        public override void ApplyChanges()
        {
            _model.IsAntiAliasingEnabled.Value = _isAntiAliasingEnabled.Value;
            SetCachedSettings();
        }

        public override void ResetSettings()
        {
            _model.Brightness.Value = _brightnessCached;
            _model.IsHighBloomQuality.Value = _isHighBloomQualityCached;
            _model.IsAntiAliasingEnabled.Value = _isAntiAliasingEnabledCached;
        }

        public void SetBrightness(float brightness)
        {
            _model.Brightness.Value = brightness;
        }

        public void SetIsHighBloomQuality(bool isHighBloomQuality)
        {
            _model.IsHighBloomQuality.Value = isHighBloomQuality;
        }

        public void SetIsAntiAliasingEnabled(bool enabled) // lazy setting (set only after applying)
        {
            _isAntiAliasingEnabled.Value = enabled;
        }

        protected override void SetCachedSettings()
        {
            _brightnessCached = _model.Brightness.Value;
            _isHighBloomQualityCached = _model.IsHighBloomQuality.Value;
            _isAntiAliasingEnabledCached = _model.IsAntiAliasingEnabled.Value;
        }

        public override void Dispose()
        {
            _disposables.Dispose();
        }
    }
}