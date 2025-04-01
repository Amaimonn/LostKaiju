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
        private int _languageCached;

        public LanguageSectionViewModel(SettingsModel model) : base(model)
        {
            CacheSettings();
            
            _languageIndex = new ReactiveProperty<int>(_languageCached);
            model.LanguageIndex.Skip(1).Subscribe(x => _languageIndex.Value = x).AddTo(_disposables);

            IsAnyChanges = Observable.CombineLatest(
                    _languageIndex.Select(x => x != _languageCached)
                ).Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }


        public override void ApplyChanges()
        {
            _model.LanguageIndex.Value = _languageIndex.Value;
            CacheSettings();
        }

        public override void CancelChanges()
        {
            _languageIndex.Value = _languageCached;
        }

        public void SetLanguage(int languageIndex)
        {
            Debug.Log($"Language in vm: {languageIndex}");
            _languageIndex.Value = languageIndex;
        }

        protected override void CacheSettings()
        {
            _languageCached = _model.LanguageIndex.Value;
        }
    }
}