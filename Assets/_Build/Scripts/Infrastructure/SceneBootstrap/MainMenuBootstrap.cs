using System;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.UI.MVVM.MainMenu;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Game.GameData.Settings;


namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MainMenuBootstrap : LifetimeScope
    {
        [SerializeField] private MainMenuView _mainMenuViewPrefab;
        [SerializeField] private string _settingsDataPath;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var exitSignal = new Subject<Unit>();

            var rootUIBinder = Container.Resolve<IRootUIBinder>();
            var gameStateProvider = Container.Resolve<IGameStateProvider>();
            var settingsModelFactory = new Func<SettingsModel>(() => SettingsModelFactory(gameStateProvider));
            var mainMenuViewModel = new MainMenuViewModel(exitSignal, settingsModelFactory, 
                rootUIBinder);
            
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

        public SettingsModel SettingsModelFactory(IGameStateProvider gameStateProvider)
        {
            var settingsData = Resources.Load<FullSettingsDataSO>(_settingsDataPath);
            var settingsModel = new SettingsModel(gameStateProvider.Settings, settingsData);
            return settingsModel;
        }
    }
}