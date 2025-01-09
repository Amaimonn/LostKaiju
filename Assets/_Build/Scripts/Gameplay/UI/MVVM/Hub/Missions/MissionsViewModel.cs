using UnityEngine;
using R3;

using LostKaiju.Models.UI.MVVM;

namespace LostKaiju.Gameplay.UI.MVVM.Hub
{
    public class MissionsViewModel : IViewModel
    {
        public Observable<bool> OnOpenStateChanged => _isOpened;
        public Observable<Unit> OnCloseCompleted => _closeCompleted;

        private readonly Subject<Unit> _startMissionSubject;
        private readonly ReactiveProperty<bool> _isOpened = new(false);
        private readonly Subject<Unit> _closeCompleted = new();
        
        public MissionsViewModel(Subject<Unit> startMissionSubject)
        {
            _startMissionSubject = startMissionSubject;
        }

        public void StartGameplay()
        {
            Debug.Log("vm start gameplay");
            _startMissionSubject.OnNext(Unit.Default);
        }

        public void CompleteClose()
        {
            _closeCompleted.OnNext(Unit.Default);
        }

        public void Open()
        {
            _isOpened.Value = true;
        }

        public void Close()
        {
            _isOpened.Value = false;
        }
    }
}