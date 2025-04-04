using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;
using R3;

using LostKaiju.Utils;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Infrastructure.Loading;
using LostKaiju.Services.Inputs;
using LostKaiju.Services.Saves;
using LostKaiju.Services.Audio;
using LostKaiju.Game.GameData.Settings;
using LostKaiju.Game.UI.MVVM.Shared.Settings;
using LostKaiju.Game.Providers.DefaultState;
using LostKaiju.Game.Providers.GameState;

namespace LostKaiju.Infrastructure.Scopes
{
    public class RootScope : LifetimeScope
    {
        [SerializeField] private RootUIBinder _uiRootBinderPrefab;

        protected override async void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(gameObject);

            builder.RegisterInstance<IInputProvider>(new InputSystemProvider());

            var monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(monoHook);
            builder.RegisterInstance(monoHook);

            var uiRootBinder = Instantiate(_uiRootBinderPrefab);
            DontDestroyOnLoad(uiRootBinder);
            builder.RegisterInstance<IRootUIBinder>(uiRootBinder);
            
            var defaultStateProvider = new DefaultStateSOProvider();
#if UNITY_EDITOR || !WEB_BUILD && (DESKTOP_BUILD || MOBILE_BUILD)
            var serizlizer = new JsonUtilityAsyncSerializer();
            var storage = new AsyncFileStorage(fileExtension: "json");
            var saveSystem = new SimpleSaveSystem(serizlizer, storage);
            var gameStateProvider = new GameStateProvider(saveSystem, defaultStateProvider);
#elif YG_BUILD
            var gameStateProvider = new GameStateProviderYG(defaultStateProvider);  
#endif
            var campaignTask =  gameStateProvider.LoadCampaignAsync();
            var settingsTask = gameStateProvider.LoadSettingsAsync();
            var heroesTask = gameStateProvider.LoadHeroesAsync();
            await campaignTask; 
            await settingsTask;
            await heroesTask;

            builder.RegisterInstance<IGameStateProvider>(gameStateProvider);

            var settingsModel = new SettingsModel(gameStateProvider.Settings);
            var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            settingsModel.IsAntiAliasingEnabled.Subscribe(x => urpAsset.msaaSampleCount = x ? 2 : 1);
            settingsModel.LanguageIndex
                .Where(x => x >= 0 && x < LocalizationSettings.AvailableLocales.Locales.Count)
                .Subscribe(l => 
                {
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[l];
                });
            builder.RegisterInstance<SettingsModel>(settingsModel);
            builder.Register<SettingsBinder>(Lifetime.Singleton);

            var loadingScreen = uiRootBinder.GetComponentInChildren<LoadingScreen>();
            builder.RegisterInstance<ILoadingScreenNotifier>(loadingScreen);

            var sceneLoader = new SceneLoader(monoHook, loadingScreen, this);
            builder.Register<AudioPlayer>(resolver => 
            {
                var settingsModel = resolver.Resolve<SettingsModel>();
                var audioPlayer = new AudioPlayer(musicVolume: settingsModel.MusicVolume.Select(x => x / 10.0f), 
                    sfxVolume: settingsModel.SfxVolume.Select(x => x / 10.0f), monoHook);
                loadingScreen.OverlayFillProgress.Subscribe(x => audioPlayer.VolumeMultiplier.Value = 1 - x);
                sceneLoader.OnLoadingStarted.Subscribe(_ => 
                {
                    audioPlayer.ClearPoolSFX();
                    audioPlayer.PauseMusic();
                });
                sceneLoader.OnLoadingFinished.Subscribe(_ =>
                {
                    audioPlayer.UnPauseMusic();
                });
                return audioPlayer;
            }, Lifetime.Singleton);

            monoHook.StartCoroutine(sceneLoader.LoadStartScene());
        }
    }
}