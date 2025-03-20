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
using LostKaiju.Game.Providers.DefaultState;
using System.Threading.Tasks;
using LostKaiju.Services.Audio;
using R3;

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
            var defaultStateProvider = new DefaultStateSOProvider();
#if UNITY_EDITOR || !YG_BUILD && !WEB_BUILD && (DESKTOP_BUILD || MOBILE_BUILD)
            var serizlizer = new JsonUtilitySerializer();
            var storage = new FileStorage(fileExtension: "json");
            var saveSystem = new SimpleSaveSystem(serizlizer, storage);
            var gameStateProvider = new GameStateProvider(saveSystem, defaultStateProvider);
#elif YG_BUILD
            var gameStateProvider = new GameStateProviderYG(defaultStateProvider);  
#endif
            var campaignTask =  gameStateProvider.LoadCampaignAsync();
            
            var settingsTask = gameStateProvider.LoadSettingsAsync();
            await Task.WhenAll(campaignTask, settingsTask);

            builder.RegisterInstance<IGameStateProvider>(gameStateProvider);

            builder.Register<SettingsModel>(resolver => 
                new SettingsModel(resolver.Resolve<IGameStateProvider>().Settings), Lifetime.Singleton);
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
                    audioPlayer.ClearSFX();
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