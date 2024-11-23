using System;
using UnityEngine;
using UnityEngine.UI;

namespace LostKaiju.UI.MVVM.MainMenu
{
    public class MainMenuView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _playButton;

        private MainMenuViewModel _viewModel;
        // private CompositeDisposable _disposables;

        public void Bind(MainMenuViewModel viewModel)
        {
            _viewModel = viewModel;
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        public void OnPlayButtonClicked()
        {
            _viewModel.StartGameplay();
        }

        #region MonoBehaviour
        private void OnDestroy()
        {
            Dispose();
        }
        #endregion

        public void Dispose()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
    }
}