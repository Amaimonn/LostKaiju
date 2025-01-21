using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.MVVM.MainMenu
{
    public class MainMenuView : ToolkitView<MainMenuViewModel>
    {
        [SerializeField] private string _playButtonName = "PlayButton";
        private Button _playButton;

        protected override void OnBind(MainMenuViewModel viewModel)
        {
            _playButton = _root.Q<Button>(name: _playButtonName);
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
            _root?.RemoveFromHierarchy();
        }
        #endregion

        public override void Dispose()
        {
            _playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
        }
    }
}