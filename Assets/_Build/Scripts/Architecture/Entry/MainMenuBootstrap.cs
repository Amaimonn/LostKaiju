using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;
using LostKaiju.UI.MVVM.MainMenu;
using UnityEngine.UIElements;
using LostKaiju.Architecture.Services;
using LostKaiju.UI.MVVM;

namespace LostKaiju.Architecture.Entry
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        // [SerializeField] private Canvas _canvas;
        // [SerializeField] private UIDocument _document;
        [SerializeField] private GameObject _mainMenuUI;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var exitSignal = new Subject<Unit>();
            var mainMenuModel = new MainMenuModel();
            var mainMenuViewModel = new MainMenuViewModel(exitSignal);
            
            mainMenuViewModel.Bind(mainMenuModel);

            var rootBinder = ServiceLocator.Current.Get<UIRootBinder>();
            var mainMenuView = Instantiate(_mainMenuUI).GetComponent<MainMenuView>();

            rootBinder.SetView(mainMenuView);
            mainMenuView.Bind(mainMenuViewModel);
            // define context in UI
            var hubEnterContext = new HubEnterContext();
            var mainMenuExitContext = new MainMenuExitContext(hubEnterContext);
            var mainMenuExitSignal = exitSignal.Select(_ => mainMenuExitContext);

            return mainMenuExitSignal;
        }
    }
}