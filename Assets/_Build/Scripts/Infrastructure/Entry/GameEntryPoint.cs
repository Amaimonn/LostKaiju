using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

using LostKaiju.Infrastructure.Scopes;
using LostKaiju.Game.Constants;
using LostKaiju.Utils;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Infrastructure.Loading;

namespace LostKaiju.Infrastructure.Entry
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

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

            var rootUiBinderPrefeb = Resources.Load<RootUIBinder>(Paths.ROOT_UI_BINDER);
            var uiRootBinder = Object.Instantiate(rootUiBinderPrefeb);
            Object.DontDestroyOnLoad(uiRootBinder);

            var loadingScreen = uiRootBinder.GetComponentInChildren<LoadingScreen>();
            loadingScreen.Show(showText: false); // no locales loaded yet

            monoHook.StartCoroutine(LoadEntryScene());
            
            IEnumerator LoadEntryScene()
            {
                if (SceneManager.GetActiveScene().name != Scenes.ENTRY_POINT)
                    yield return SceneManager.LoadSceneAsync(Scenes.ENTRY_POINT);
                Debug.Log("Entry point scene loaded");
                yield return LocalizationSettings.InitializationOperation;

                var rootScope = Object.FindAnyObjectByType<RootScope>();
                rootScope.SetDependencies(uiRootBinder, loadingScreen);
                rootScope.Build();
                Object.Destroy(monoHook.gameObject);
            }
        }
    }
}
