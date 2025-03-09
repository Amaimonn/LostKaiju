using UnityEngine;
using VContainer;
using VContainer.Unity;

using LostKaiju.Utils;
using LostKaiju.Infrastructure.Loading;
using LostKaiju.Services.Inputs;
using LostKaiju.Services.Saves;
using LostKaiju.Game.Providers.GameState;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.GameData.Campaign;
using LostKaiju.Game.Constants;
using LostKaiju.Game.Providers.DefaultState;

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
            // builder.Register<IDefaultStateProvider, DefaultStateSOProvider>(Lifetime.Singleton);
            var defaultStateProvider = new DefaultStateSOProvider();
#if UNITY_EDITOR || !YG_BUILD && (DESKTOP_BUILD || MOBILE_BUILD)
            var serizlizer = new JsonUtilitySerializer();
            var storage = new FileStorage(fileExtension: "json");
            var saveSystem = new SimpleSaveSystem(serizlizer, storage);
            // builder.RegisterInstance<ISaveSystem>(saveSystem);
            var gameStateProvider = new GameStateProvider(saveSystem, defaultStateProvider);
#elif YG_BUILD
            var gameStateProvider = new GameStateProviderYG(defaultStateProvider);  
#endif
            var campaignTask =  gameStateProvider.LoadCampaignAsync();
            
            var settingsTask = gameStateProvider.LoadSettingsAsync();
            await campaignTask;

            if (gameStateProvider.Campaign.LastUpdateIndex != GameInfo.CampaignUpdateIndex)
            {
                var campaignModelFactory = new CampaignModelFactory();
                var campaignModel = await campaignModelFactory.GetModelAsync(gameStateProvider);
                campaignModel.UpdateAvailableLocationsAndMissions();
                gameStateProvider.Campaign.LastUpdateIndex = GameInfo.CampaignUpdateIndex;
                await gameStateProvider.SaveCampaignAsync();
            }

            await settingsTask;

            builder.RegisterInstance<IGameStateProvider>(gameStateProvider);

            builder.Register<SettingsModel>(resolver => 
                new SettingsModel(resolver.Resolve<IGameStateProvider>().Settings), Lifetime.Singleton);
            builder.Register<SettingsBinder>(Lifetime.Singleton);

            var loadingScreen = uiRootBinder.GetComponentInChildren<LoadingScreen>();
            var sceneLoader = new SceneLoader(monoHook, loadingScreen, this);
            monoHook.StartCoroutine(sceneLoader.LoadStartScene());
        }
    }
}