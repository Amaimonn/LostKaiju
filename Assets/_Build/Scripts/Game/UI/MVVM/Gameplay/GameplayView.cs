using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.Gameplay
{
    public class GameplayView : CanvasView<GameplayViewModel>
    {
        [SerializeField] private Button _optionsButton;
        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(GameplayViewModel viewModel)
        {
            _optionsButton.onClick.AddListener(ViewModel.OpenOptions);
        }
    }
}