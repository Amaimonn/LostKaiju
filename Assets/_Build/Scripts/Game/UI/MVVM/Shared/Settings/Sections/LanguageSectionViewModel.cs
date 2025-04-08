using System.Linq;
using UnityEngine;
using R3;

using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Game.UI.MVVM.Shared.Settings
{
    public class LanguageSectionViewModel : SettingsSectionViewModel
    {
        public Observable<int> LanguageIndex => _languageIndex;

        private readonly ReactiveProperty<int> _languageIndex;

        public LanguageSectionViewModel(SettingsModel model) : base(model)
        {
            // Unique language cache logic (lazy applying makes it possible to select a language from the outside)
            CacheSettings(); 
            
            _languageIndex = new ReactiveProperty<int>(_model.LanguageIndex.Value);
            model.LanguageIndex.Skip(1).Subscribe(x => _languageIndex.Value = x).AddTo(_disposables);

            IsAnyChanges = Observable.CombineLatest(
                    _languageIndex.Select(x => x != _model.LanguageIndex.Value)
                ).Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }


        public override void ApplyChanges()
        {
            if (!_model.IsLanguageSelected.Value && _model.LanguageIndex.Value != _languageIndex.Value)
                _model.IsLanguageSelected.Value = true;
            _model.LanguageIndex.Value = _languageIndex.Value;
            CacheSettings();
        }

        public override void CancelChanges()
        {
            _languageIndex.Value = _model.LanguageIndex.Value;
        }

        public void SetLanguage(int languageIndex) // lazy (no model value changes)
        {
            Debug.Log($"Language in vm: {languageIndex}");
            _languageIndex.Value = languageIndex;
        }

        // protected override void CacheSettings()
        // {
        //     _languageCached = _model.LanguageIndex.Value;
        // }
    }
}