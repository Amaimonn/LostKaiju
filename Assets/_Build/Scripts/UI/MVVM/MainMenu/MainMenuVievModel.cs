using UnityEngine;
using R3;

namespace LostKaiju.UI.MVVM.MainMenu
{
    public class MainMenuViewModel : IViewModel
    {
        private readonly Subject<Unit> _exitSubject;
        private MainMenuModel _model;

        public MainMenuViewModel(Subject<Unit> exitSubject)
        {
            _exitSubject = exitSubject;
        }

        public void Bind(MainMenuModel model)
        {
            _model = model;
        }

        public void StartGameplay()
        {
            Debug.Log("Start Gameplay signal in vm");
            _exitSubject.OnNext(Unit.Default);
        }
    }
}