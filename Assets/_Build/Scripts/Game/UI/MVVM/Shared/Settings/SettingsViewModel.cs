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
        private readonly ReactiveProperty<float> _sfxVolume;

        public SettingsViewModel(SettingsModel model)
        {
            _model = model;
            _soundVolume = new ReactiveProperty<float>(model.SoundVolume.Value);
            _sfxVolume = new ReactiveProperty<float>(model.SFXVolume.Value);
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
            _model.SFXVolume.Value = _sfxVolume.Value;
        }
    }
}