using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuUI;
        private Subject<MainMenuExitContext> _mainMenuExitSignal;

        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            _mainMenuExitSignal = new Subject<MainMenuExitContext>();
            return _mainMenuExitSignal;
        }

        public void EnterTheHub()
        {
            var hubEnterContext = new HubEnterContext();
            _mainMenuExitSignal.OnNext(new MainMenuExitContext(hubEnterContext));
        }
    }
}
