using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using LostKaiju.Infrastructure.Scopes;
using LostKaiju.Game.Constants;
using LostKaiju.Utils;

namespace LostKaiju.Infrastructure.Entry
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private const float LOGO_TIME = 0.8f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnterTheGame()
        {
            _instance = new();
            _instance.Run();
        }

        private void Run()
        {
            var monoHook = new GameObject("EntryMonoHook").AddComponent<MonoBehaviourHook>();
            Object.DontDestroyOnLoad(monoHook);
            monoHook.StartCoroutine(LoadEntryScene());
            
            IEnumerator LoadEntryScene()
            {
                if (SceneManager.GetActiveScene().name != Scenes.ENTRY_POINT)
                    yield return SceneManager.LoadSceneAsync(Scenes.ENTRY_POINT);
                Debug.Log("Entry point scene loaded");
                
                yield return new WaitForSeconds(LOGO_TIME);
                Object.FindAnyObjectByType<RootScope>().Build();

                Object.Destroy(monoHook.gameObject);
            }
        }
    }
}
