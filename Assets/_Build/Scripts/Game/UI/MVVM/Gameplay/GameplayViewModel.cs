using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.Gameplay.PlayerCreature;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class GameplayViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSignal;
        private readonly PlayerIndicatorsViewModel _playerIndicatorsViewModel;

        public GameplayViewModel(Subject<Unit> exitSignal)
        {
            _exitSignal = exitSignal;
        }
    }
}