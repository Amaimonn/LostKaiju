using UnityEngine;

using LostKaiju.Models.UI.MVVM;
using LostKaiju.Models.Locator;
using LostKaiju.Utils;
using LostKaiju.Infrastructure.Loading;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    /// <summary>
    /// Scene with logic for the game initialization. Runs only once at the beginning.
    /// </summary>
    public class EntryBootstrap : MonoBehaviour
    {
        [SerializeField] private RootUIBinder _uiRootBinderPrefab;
        private MonoBehaviourHook _monoHook;
        
        public void Boot()
        {
            _monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(_monoHook);

            var serviceLocator = ServiceLocator.Current;
            
            var uiRootBinder = Instantiate(_uiRootBinderPrefab);
            serviceLocator.Register<IRootUIBinder>(uiRootBinder);
            DontDestroyOnLoad(uiRootBinder);
            
            var loadingScreen = uiRootBinder.GetComponentInChildren<LoadingScreen>();
            var sceneLoader = new SceneLoader(_monoHook, loadingScreen);
            _monoHook.StartCoroutine(sceneLoader.LoadStartScene());
        }
    }
}
