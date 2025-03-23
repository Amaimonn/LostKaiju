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
                yield return SceneManager.LoadSceneAsync(Scenes.ENTRY_POINT);
                yield return new WaitForSeconds(LOGO_TIME);
                Object.FindAnyObjectByType<RootScope>().Build();

                Debug.Log("Entry point scene loaded");
                Object.Destroy(monoHook.gameObject);
            }
        }
    }
}
