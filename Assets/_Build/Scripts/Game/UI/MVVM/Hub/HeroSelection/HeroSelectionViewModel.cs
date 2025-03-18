using System;
using LostKaiju.Game.GameData.Heroes;
using R3;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HeroSelectionViewModel : ScreenViewModel, IDelayedBinding, IDisposable
    {
        public Observable<IHeroData[]> AllHeroesData => _allHeroesData;
        public Observable<IHeroData> CurrentHeroDataPreview => _currentHeroDataPreview;
        public Observable<IHeroData> SelectedHeroData => _heroesModel.SelectedHeroData;
        public Observable<bool> IsLoaded => _isLoaded;

        private HeroesModel _heroesModel;
        private readonly ReactiveProperty<bool> _isLoaded = new(false);
        private readonly ReactiveProperty<IHeroData[]> _allHeroesData = new(null);
        private readonly ReactiveProperty<IHeroData> _currentHeroDataPreview = new(null);
        private readonly ReactiveProperty<IHeroData> _selectedHeroData = new(null);

        public void Bind(HeroesModel heroesModel)
        {
            _heroesModel = heroesModel;
            _allHeroesData.Value = heroesModel.AllHeroesData.AllData;
            _currentHeroDataPreview.Value = _heroesModel.SelectedHeroData.Value;
            _selectedHeroData.Value = _heroesModel.SelectedHeroData.Value; 
            _isLoaded.Value = true;
        }

        public void CompleteSelection()
        {
            _heroesModel.SelectedHeroData.Value = _currentHeroDataPreview.Value;
            StartClosing();
        }

        public void PreviewHeroSlot(string heroId)
        {
            _currentHeroDataPreview.Value = _heroesModel.HeroDataMap[heroId];
        }

        public void Dispose()
        {
        }
    }
}