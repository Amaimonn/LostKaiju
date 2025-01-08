using UnityEngine;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Gameplay.UI.MVVM.MainMenu;
using LostKaiju.Models.Locator;
using LostKaiju.Models.UI.MVVM;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuUI;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var exitSignal = new Subject<Unit>();
            var mainMenuModel = new MainMenuModel();
            var mainMenuViewModel = new MainMenuViewModel(exitSignal);
            
            mainMenuViewModel.Bind(mainMenuModel);

            var rootBinder = ServiceLocator.Current.Get<UIRootBinder>();
            var mainMenuView = Instantiate(_mainMenuUI).GetComponent<MainMenuView>();

            mainMenuView.Bind(mainMenuViewModel);
            rootBinder.SetViews(mainMenuView);
            
            // define context in UI
            var hubEnterContext = new HubEnterContext();
            var mainMenuExitContext = new MainMenuExitContext(hubEnterContext);
            var mainMenuExitSignal = exitSignal.Select(_ => mainMenuExitContext);

            return mainMenuExitSignal;
        }
    }
}