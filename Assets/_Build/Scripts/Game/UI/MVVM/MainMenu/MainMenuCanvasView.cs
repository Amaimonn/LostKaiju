using UnityEngine;
using UnityEngine.UI;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.MainMenu
{
    public class MainMenuCanvasView : CanvasView<MainMenuViewModel>
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button  _settingsButton;

        protected override CanvasOrder Order => CanvasOrder.First;

        protected override void OnBind(MainMenuViewModel viewModel)
        {
            _playButton.onClick.AddListener(StartGameplay);
            _settingsButton.onClick.AddListener(OpenSettings);
        }

        public void StartGameplay()
        {
            ViewModel.StartGameplay();
            _playButton.onClick.RemoveListener(StartGameplay);
        }

        public void OpenSettings()
        {
            ViewModel.OpenSettings();
        }
    }
}