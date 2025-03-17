using UnityEngine;
using UnityEngine.UI;
using R3;

namespace LostKaiju.Game.UI.MVVM
{
    public abstract class PopUpCanvasView<T> : ScreenCanvasView<T> where T : ScreenViewModel
    {
        [SerializeField] protected Button _closeButton;
        [SerializeField] protected Button _closeBackground;

        protected override void OnBind(T viewModel)
        {
            base.OnBind(viewModel);
            
            if (_closeButton != null)
                _closeButton.onClick.AsObservable().Take(1).Subscribe(_ => viewModel.StartClosing());
            if (_closeBackground != null)
                _closeBackground.onClick.AsObservable().Take(1).Subscribe(_ => viewModel.StartClosing());
        }
    }
}