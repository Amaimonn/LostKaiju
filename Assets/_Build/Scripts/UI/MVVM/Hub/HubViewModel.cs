using UnityEngine;
using R3;

namespace LostKaiju.UI.MVVM.Hub
{
    public class HubViewModel : IViewModel
    {
        private Subject<Unit> _exitSubject;
        public HubViewModel(Subject<Unit> exitSubject)
        {
            _exitSubject = exitSubject;
        }

        public void StartGameplay()
        {
            Debug.Log("vm start gameplay");
            _exitSubject.OnNext(Unit.Default);
        }
    }
}