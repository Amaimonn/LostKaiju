using System.Linq;
using R3;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class VideoSettingsViewModel : SettingsSectionViewModel
    {
        public Observable<int> Brightness => _brightness;
        public Observable<bool> IsPostProcessingEnabled => _isPostProcessingEnabled;
        public Observable<bool> IsBloomEnabled => _isBloomEnabled;
        public Observable<bool> IsFilmGrainEnabled => _isFilmGrainEnabled;
        public Observable<bool> IsAntiAliasingEnabled => _isAntiAliasingEnabled;

        private readonly ReactiveProperty<int> _brightness;
        private readonly ReactiveProperty<bool> _isPostProcessingEnabled;
        private readonly ReactiveProperty<bool> _isBloomEnabled;
        private readonly ReactiveProperty<bool> _isFilmGrainEnabled;
        private readonly ReactiveProperty<bool> _isAntiAliasingEnabled;

        private int _brightnessCached;
        private bool _isPostProcessingEnabledCached;
        private bool _isBloomEnabledCached;
        private bool _isFilmGrainEnabledCached;
        private bool _isAntiAliasingEnabledCached;

        public VideoSettingsViewModel(SettingsModel model) : base(model)
        {
            CacheSettings();

            _brightness = new ReactiveProperty<int>(model.Brightness.Value);
            model.Brightness.Skip(1).Subscribe(x => _brightness.Value = x).AddTo(_disposables);
            
            _isPostProcessingEnabled = new ReactiveProperty<bool>(model.IsPostProcessingEnabled.Value);
            model.IsPostProcessingEnabled.Skip(1).Subscribe(x => _isPostProcessingEnabled.Value = x).AddTo(_disposables);

            _isBloomEnabled = new ReactiveProperty<bool>(model.IsBloomEnabled.Value);
            model.IsBloomEnabled.Skip(1).Subscribe(x => _isBloomEnabled.Value = x).AddTo(_disposables);

            _isFilmGrainEnabled = new ReactiveProperty<bool>(model.IsFilmGrainEnabled.Value);
            model.IsFilmGrainEnabled.Skip(1).Subscribe(x => _isFilmGrainEnabled.Value = x).AddTo(_disposables);

            _isAntiAliasingEnabled = new ReactiveProperty<bool>(model.IsAntiAliasingEnabled.Value);
            model.IsAntiAliasingEnabled.Skip(1).Subscribe(x => _isAntiAliasingEnabled.Value = x).AddTo(_disposables);

            IsAnyChanges = Observable.CombineLatest(
                _brightness.Select(x => x != _brightnessCached),
                _isPostProcessingEnabled.Select(x => x != _isPostProcessingEnabledCached),
                _isBloomEnabled.Select(x => x != _isBloomEnabledCached),
                _isFilmGrainEnabled.Select(x => x != _isFilmGrainEnabledCached),
                _isAntiAliasingEnabled.Select(x => x != _isAntiAliasingEnabledCached)
            ).Select(x => x.Any(t => t == true))
            .ToReadOnlyReactiveProperty();
        }


        public override void ApplyChanges()
        {
            _model.IsAntiAliasingEnabled.Value = _isAntiAliasingEnabled.Value;
            CacheSettings();
        }

        public override void CancelChanges()
        {
            _model.Brightness.Value = _brightnessCached;
            _model.IsPostProcessingEnabled.Value = _isPostProcessingEnabledCached;
            _model.IsBloomEnabled.Value = _isBloomEnabledCached;
            // force callback even if value in Model hasn't changed to make UI syncronized
            _isAntiAliasingEnabled.Value = _isAntiAliasingEnabledCached;
            // _model.IsAntiAliasingEnabled.OnNext(_isAntiAliasingEnabledCached); 
        }

        public void SetBrightness(int brightness)
        {
            _model.Brightness.Value = brightness;
        }

        public void SetIsPostProcessingEnabled(bool enabled)
        {
            _model.IsPostProcessingEnabled.Value = enabled;
        }

        public void SetIsBloomEnabled(bool isHighBloomQuality)
        {
            _model.IsBloomEnabled.Value = isHighBloomQuality;
        }

        public void SetIsFilmGrainEnabled(bool isFilmGrainEnabled)
        {
            _model.IsFilmGrainEnabled.Value = isFilmGrainEnabled;
        }

        public void SetIsAntiAliasingEnabled(bool enabled) // lazy setting (set only after applying)
        {
            _isAntiAliasingEnabled.Value = enabled;
        }

        protected override void CacheSettings()
        {
            _brightnessCached = _model.Brightness.Value;
            _isPostProcessingEnabledCached = _model.IsPostProcessingEnabled.Value;
            _isBloomEnabledCached = _model.IsBloomEnabled.Value;
            _isFilmGrainEnabledCached = _model.IsFilmGrainEnabled.Value;
            _isAntiAliasingEnabledCached = _model.IsAntiAliasingEnabled.Value;
        }
    }
}