using UnityEngine;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Utils;

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
        }
    }
}
