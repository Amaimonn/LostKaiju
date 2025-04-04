using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Game.UI.MVVM.MainMenu
{
    public class MainMenuView : ToolkitView<MainMenuViewModel>
    {
        [SerializeField] private string _playButtonName = "PlayButton";
        [SerializeField] private string _settingsButtonName = "SettingsButton";

        private Button _playButton;
        private Button _settingsButton;

        protected override void OnBind(MainMenuViewModel viewModel)
        {
            _playButton = Root.Q<Button>(name: _playButtonName);
            _settingsButton = Root.Q<Button>(name: _settingsButtonName);

            _playButton.RegisterCallbackOnce<ClickEvent>(StartGameplay);
            _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);
        }

        public void StartGameplay(ClickEvent clickEvent)
        {
            ViewModel.StartGameplay();
        }

        public void OpenSettings(ClickEvent clickEvent)
        {
            ViewModel.OpenSettings();
        }

        // public override void Dispose()
        // {
        //     _playButton.UnregisterCallback<ClickEvent>(StartGameplay);
        // }
    }
}