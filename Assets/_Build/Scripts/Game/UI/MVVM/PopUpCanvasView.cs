using UnityEngine;
using UnityEngine.UI;
using R3;
using LostKaiju.Services.Audio;

namespace LostKaiju.Game.UI.MVVM
{
    public abstract class PopUpCanvasView<T> : ScreenCanvasView<T> where T : ScreenViewModel
    {
        [SerializeField] protected Button _closeButton;
        [SerializeField] protected Button _closeBackground;
        protected AudioPlayer _audioPlayer;

        public virtual void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

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