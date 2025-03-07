using System;
using System.Collections.Generic;
using System.Linq;
using R3;

using LostKaiju.Game.GameData.SettingsDyn;
using LostKaiju.Game.Providers.GameState;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    public class SettingsViewModel : ScreenViewModel, IDisposable
    {
        public SettingsSectionViewModel CurrentSection => _currentSection;
        public readonly Dictionary<string, SettingsSectionViewModel> Sections = new ();
        public readonly ISettingsData SettingsData;
        public ReadOnlyReactiveProperty<bool> IsAnyChanges;

        private readonly IGameStateProvider _gameStateProvider;
        private SettingsSectionViewModel _currentSection;

        public SettingsViewModel(SettingsModel model, ISettingsData settingsData,
            IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
            SettingsData = settingsData;

            var sectionsMap = new Dictionary<string, ISettingsSectionData>();
            foreach (var sectionData in settingsData.SectionsData)
            {
                sectionsMap.Add(sectionData.Id, sectionData);
            }

            var soundSectionViewModel = new SoundSettingsViewModel(model, sectionsMap["Sound"]);
            Sections.Add("Sound", soundSectionViewModel);
            Sections.Add("Video", new VideoSettingsViewModel(model, sectionsMap["Video"]));
            
            _currentSection = soundSectionViewModel;
            IsAnyChanges = Observable.CombineLatest(Sections.Values.Select(s => s.IsAnyChanges))
                .Select(x => x.Any(t => t == true))
                .ToReadOnlyReactiveProperty();
        }

        public override void StartClosing()
        {
            ResetUnappliedChanges();
            base.StartClosing();
        }

        public void SelectSection(string sectionId)
        {
            _currentSection = Sections[sectionId];
        }

        public void ApplyChanges()
        {
            foreach (var section in Sections.Values)
                section.ApplyChanges();
            SaveSettings();
        }

        public void ResetUnappliedChanges()
        {
            foreach (var section in Sections.Values)
                section.ResetSettings();
        }

        public void ResetCurrentSectionSettings()
        {
            _currentSection.ResetSettings();
        }

        private void SaveSettings()
        {
            _gameStateProvider.SaveSettingsAsync();
        }

        public void Dispose()
        {
            foreach (var section in Sections.Values)
                section.Dispose();
        }
    }
}