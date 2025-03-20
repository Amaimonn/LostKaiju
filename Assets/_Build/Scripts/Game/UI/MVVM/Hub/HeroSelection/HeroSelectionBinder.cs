using UnityEngine;
using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Constants;
using LostKaiju.Game.GameData.Heroes;
using LostKaiju.Game.GameData;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HeroSelectionBinder : Binder<HeroSelectionViewModel>
    {
        private readonly ILoadableModelFactory<HeroesModel> _factory;

        public HeroSelectionBinder(IRootUIBinder rootUIBinder, ILoadableModelFactory<HeroesModel> factory) : 
            base(rootUIBinder)
        {
            _factory = factory;
        }

        public override bool TryBindAndOpen(out HeroSelectionViewModel viewModel)
        {
            if (_currentViewModel != null) // if already exists
            {
                viewModel = null;
                return false;
            }

            var heroesDataSO = Resources.Load<AllHeroesDataSO>(Paths.ALL_HEROES_DATA);
            var heroSelectionView = LoadAndInstantiateView<HeroSelectionView>(Paths.HERO_SELECTION_VIEW);
            
            _currentViewModel = new HeroSelectionViewModel();
            
            _currentViewModel.OnClosingCompleted.Subscribe(_ => {
                _rootUIBinder.ClearView(heroSelectionView);
            });
            BindHeroesOnLoaded();
            heroSelectionView.OnDisposed.Take(1).Subscribe(_ => {
                _currentViewModel?.Dispose();
                _currentViewModel = null;
            });

            heroSelectionView.Bind(_currentViewModel);
            _rootUIBinder.AddView(heroSelectionView);
            _currentViewModel.Open();
            _onOpened.OnNext(_currentViewModel);
            
            viewModel = _currentViewModel;
            return true;

            void BindHeroesOnLoaded()
            {
                _factory.GetModelOnLoaded(heroesModel =>
                {
                    if (_currentViewModel != null && heroSelectionView != null)
                    {
                        _currentViewModel.Bind(heroesModel);
                    }
                });
                
            }
        }
    }
}