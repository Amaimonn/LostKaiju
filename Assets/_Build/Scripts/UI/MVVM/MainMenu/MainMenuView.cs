using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.UI.MVVM.MainMenu
{
    public class MainMenuView : MonoBehaviour, IDisposable
    {
        [SerializeField] private VisualTreeAsset _visualTree;
        [SerializeField] private string _playButtonName = "PlayButton";

        private VisualElement _root;
        private Button _playButton;
        private MainMenuViewModel _viewModel;

        public void Initialize(UIDocument document)
        {
            _root = _visualTree.CloneTree();
            document.rootVisualElement.Q<VisualElement>(name: "UIRoot").Add(_root);
            _root.style.position = Position.Absolute;
            _root.style.width = Length.Percent(100);
            _root.style.height = Length.Percent(100);
            _playButton = _root.Q<Button>(name: _playButtonName);
        }

        public void Bind(MainMenuViewModel viewModel)
        {
            _viewModel = viewModel;
            _playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        }

        public void OnPlayButtonClicked(ClickEvent clickEvent)
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
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
        }
    }
}