using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.MainMenu;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Services.Inputs;
using LostKaiju.Services.Audio;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MainMenuBootstrap : LifetimeScope
    {
        [SerializeField] private MainMenuView _mainMenuViewPrefab;
        // [SerializeField] private MainMenuCanvasView _mainMenuViewPrefab;
        [SerializeField] private AudioClip _musicClip;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var exitSignal = new Subject<Unit>();

            var audioPlayer = Container.Resolve<AudioPlayer>();
            audioPlayer.PlayMusic(_musicClip, true);

            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var settingsBinder = Container.Resolve<SettingsBinder>();
            var inputProvider = Container.Resolve<IInputProvider>();
            settingsBinder.BindClosingSignal(inputProvider.OnEscape.TakeUntil(exitSignal));
            var mainMenuViewModel = new MainMenuViewModel(exitSignal, settingsBinder);
            var rootBinder = Container.Resolve<IRootUIBinder>();
            var mainMenuView = Instantiate(_mainMenuViewPrefab);

            mainMenuView.Bind(mainMenuViewModel);
            rootBinder.SetView(mainMenuView);
            
            // define context in UI
            var hubEnterContext = new HubEnterContext();
            var mainMenuExitContext = new MainMenuExitContext(hubEnterContext);
            var mainMenuExitSignal = exitSignal.Select(_ => mainMenuExitContext);

            return mainMenuExitSignal;
        }
    }
}