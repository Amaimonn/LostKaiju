using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.MainMenu;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Settings;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MainMenuBootstrap : LifetimeScope
    {
        [SerializeField] private MainMenuView _mainMenuViewPrefab;
        [SerializeField] private string _settingsDataPath;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SettingsModel>(resolver => 
                new SettingsModel(resolver.Resolve<IGameStateProvider>().Settings), Lifetime.Singleton);
            builder.Register<SettingsBinder>(resolver => {
                var settingsBinder = new SettingsBinder(_settingsDataPath);
                resolver.Inject(settingsBinder);
                return settingsBinder;
            }, Lifetime.Singleton);
        }

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var exitSignal = new Subject<Unit>();

            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var settingsBinder = Container.Resolve<SettingsBinder>();
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