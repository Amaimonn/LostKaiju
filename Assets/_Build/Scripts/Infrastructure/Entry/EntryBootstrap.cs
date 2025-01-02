using UnityEngine;

using LostKaiju.Models.UI.MVVM;
using LostKaiju.Models.Locator;
using LostKaiju.Utils;

namespace LostKaiju.Infrastructure.Entry
{
    /// <summary>
    /// Scene with logic for the game initialization. Runs only once at the beginning.
    /// </summary>
    public class EntryBootstrap : MonoBehaviour
    {
        [SerializeField] private UIRootBinder _uiRootBinderPrefab;
        private MonoBehaviourHook _monoHook;
        
        public void Boot()
        {
            _monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(_monoHook);

            var uiRootBinder = Instantiate(_uiRootBinderPrefab);
            ServiceLocator.Current.Register(uiRootBinder);
            DontDestroyOnLoad(uiRootBinder);

            var sceneLoader = new SceneLoader(_monoHook);
            _monoHook.StartCoroutine(sceneLoader.LoadStartScene());
        }
    }
}
