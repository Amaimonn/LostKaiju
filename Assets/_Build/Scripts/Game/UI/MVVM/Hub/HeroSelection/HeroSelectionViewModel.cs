using System;
using LostKaiju.Game.GameData.Heroes;
using R3;

namespace LostKaiju.Game.UI.MVVM.Hub
{
    public class HeroSelectionViewModel : ScreenViewModel, IDelayedBinding, IDisposable
    {
        public Observable<bool> IsLoaded => _isLoaded;

        private HeroesModel _heroesModel;
        private readonly ReactiveProperty<bool> _isLoaded = new(false);

        public void Bind(HeroesModel heroesModel)
        {
            _heroesModel = heroesModel;
            _isLoaded.Value = true;
        }

        public void CompleteSelection()
        {
            StartClosing();
        }

        public void Dispose()
        {
        }
    }
}