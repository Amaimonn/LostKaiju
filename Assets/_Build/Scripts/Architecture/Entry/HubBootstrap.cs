using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

using LostKaiju.Architecture.Entry.Context;
using System.Collections;
using LostKaiju.Player.View;
using LostKaiju.Configs;

namespace LostKaiju.Architecture.Entry
{
    public class HubBootstrap : MonoBehaviour
    {
        
        [SerializeField] private PlayerBinderSO _playerBinderSO; // choose your player character

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubExitSignal = new Subject<HubExitContext>(); // send to UI
            hubExitSignal.Subscribe(ctx => 
            {
                var toScene = ctx.ToSceneContext.SceneName;
                if (toScene == Scenes.GAMEPLAY)
                {
                    LoadGameplay(ctx);
                }
            });
            //test
            var gameplayEnterContext = new GameplayEnterContext(Scenes.GAMEPLAY)
            {
                PlayerBinderSO = _playerBinderSO
            };

            var hubExitContext= new HubExitContext(gameplayEnterContext);
            LoadGameplay(hubExitContext);
            //end test

            return hubExitSignal;
        }

        private async void LoadGameplay(HubExitContext hubExitContext)
        {
            await SceneManager.LoadSceneAsync(Scenes.GAP);
            await SceneManager.LoadSceneAsync(Scenes.GAMEPLAY);

            Debug.Log("Gameplay scene loaded");
        
            var gameplayBootstrap = FindFirstObjectByType<GameplayBootstrap>();
            var gameplayExitSignal = gameplayBootstrap.Boot(hubExitContext.ToSceneContext as GameplayEnterContext);
            // gameplayExitSignal.Subscribe(ctx =>
            // {
            //     StartCoroutine(LoadHub(ctx));
    
            // });
        }
    }
}
