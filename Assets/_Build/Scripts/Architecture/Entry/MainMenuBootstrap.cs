using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;
using LostKaiju.UI.MVVM.MainMenu;

namespace LostKaiju.Architecture.Entry
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _mainMenuUI;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var exitSignal = new Subject<Unit>();

            var mainMenuModel = new MainMenuModel();
            var mainMenuViewModel = new MainMenuViewModel(exitSignal);
            var mainMenuView = Instantiate(_mainMenuUI, _canvas.transform).GetComponent<MainMenuView>();

            mainMenuViewModel.Bind(mainMenuModel);
            mainMenuView.Bind(mainMenuViewModel);

            // define context in UI
            var hubEnterContext = new HubEnterContext();
            var mainMenuExitContext = new MainMenuExitContext(hubEnterContext);
            var mainMenuExitSignal = exitSignal.Select(_ => mainMenuExitContext);

            return mainMenuExitSignal;
        }
    }
}