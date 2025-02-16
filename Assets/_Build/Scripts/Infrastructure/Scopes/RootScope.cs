using System.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using VContainer;

using LostKaiju.Utils;
using LostKaiju.Infrastructure.Loading;
using LostKaiju.Services.Inputs;
using LostKaiju.Services.Saves;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Boilerplates.UI.MVVM;

namespace LostKaiju.Infrastructure.Scopes
{
    public class RootScope : LifetimeScope
    {
        [SerializeField] private RootUIBinder _uiRootBinderPrefab;

        protected override async void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(gameObject);

            var monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(monoHook);
            builder.RegisterInstance(monoHook);

            var uiRootBinder = Instantiate(_uiRootBinderPrefab);
            DontDestroyOnLoad(uiRootBinder);
            builder.RegisterInstance<IRootUIBinder>(uiRootBinder);
            
            builder.Register<IInputProvider, InputSystemProvider>(Lifetime.Singleton);
            
            var serizlizer = new JsonUtilitySerializer();
            var storage = new FileStorage(fileExtension: "json");
            var saveSystem = new SimpleSaveSystem(serizlizer, storage);
            builder.RegisterInstance<ISaveSystem>(saveSystem);

            var gameStateProvider = new GameStateProvider(saveSystem);
            var campaignTask =  gameStateProvider.LoadCampaignAsync();
            var settingsTask = gameStateProvider.LoadSettingsAsync();
            await Task.WhenAll(campaignTask, settingsTask);

            builder.RegisterInstance<IGameStateProvider>(gameStateProvider);

            var loadingScreen = uiRootBinder.GetComponentInChildren<LoadingScreen>();
            var sceneLoader = new SceneLoader(monoHook, loadingScreen, this);
            monoHook.StartCoroutine(sceneLoader.LoadStartScene());
        }
    }
}