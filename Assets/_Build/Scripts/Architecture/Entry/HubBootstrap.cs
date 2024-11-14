using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;
using LostKaiju.Configs;

namespace LostKaiju.Architecture.Entry
{
    public class HubBootstrap : MonoBehaviour
    {
        
        [SerializeField] private PlayerBinderSO _playerBinderSO; // choose your player character

        private Subject<HubExitContext> _hubExitSignal;

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            _hubExitSignal = new Subject<HubExitContext>(); // send to UI
            return _hubExitSignal;
        }

        public void StartGameplay()
        {
            var gameplayEnterContext = new GameplayEnterContext(Scenes.GAMEPLAY)
            {
                LevelSceneName = "Level_1_1", // level loading example
                PlayerConfig = _playerBinderSO
            };
            _hubExitSignal.OnNext(new HubExitContext(gameplayEnterContext));
        }
    }
}
