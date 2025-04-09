using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;
using R3;
using YG;

using LostKaiju.Utils;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Infrastructure.Loading;
using LostKaiju.Services.Inputs;
using LostKaiju.Services.Saves; // !WEB_BUILD
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
        private RootUIBinder _uiRootBinder;
        private LoadingScreen _loadingScreen;

        public void SetDependencies(RootUIBinder uiRootBinder, LoadingScreen loadingScreen)
        {
            _uiRootBinder = uiRootBinder;
            _loadingScreen = loadingScreen;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(gameObject);

            builder.RegisterInstance<IInputProvider>(new InputSystemProvider());

            var monoHook = new GameObject("MonoHook").AddComponent<MonoBehaviourHook>();
            DontDestroyOnLoad(monoHook);
            builder.RegisterInstance(monoHook);

            builder.RegisterInstance<IRootUIBinder>(_uiRootBinder);
            
            var defaultStateProvider = new DefaultStateSOProvider();
#if !WEB_BUILD && (DESKTOP_BUILD || MOBILE_BUILD)
            var serizlizer = new JsonUtilitySerializer();
            var storage = new FileStorage(fileExtension: "json");
            var saveSystem = new SimpleSaveSystem(serizlizer, storage);
            var gameStateProvider = new GameStateProvider(saveSystem, defaultStateProvider);
#elif YG_BUILD
            var gameStateProvider = new GameStateProviderYG(defaultStateProvider);  
#endif
            gameStateProvider.LoadCampaign();
            gameStateProvider.LoadSettings();
            gameStateProvider.LoadHeroes();

            builder.RegisterInstance<IGameStateProvider>(gameStateProvider);

            var settingsModel = new SettingsModel(gameStateProvider.Settings);
            var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            settingsModel.IsAntiAliasingEnabled.Subscribe(x => urpAsset.msaaSampleCount = x ? 2 : 1);
#if YG_BUILD
            YG2.onSwitchLang += newCode =>
            {
                // if (!settingsModel.IsLanguageSelected.Value)
                // {
                //     Debug.Log("language was changed from external tools, because it wasn`t specified in settings");
                settingsModel.LanguageIndex.Value = LocaleHelper.GetLanguageIndexByCode(newCode);
                // }
            };
            settingsModel.LanguageIndex.Subscribe(newIndex => 
            {
                var newCode = LocaleHelper.GetLanguageCodeByIndex(newIndex);
                if (YG2.lang != newCode)
                    YG2.SwitchLanguage(newCode);
            });
#endif
            settingsModel.LanguageIndex.Subscribe(x => 
            {
                SelectLanguageByIndex(x);
            });
            builder.RegisterInstance<SettingsModel>(settingsModel);
            builder.Register<SettingsBinder>(Lifetime.Singleton);

            builder.RegisterInstance<ILoadingScreenNotifier>(_loadingScreen);

            var sceneLoader = new SceneLoader(monoHook, _loadingScreen, this);
            builder.Register<AudioPlayer>(resolver => 
            {
                var settingsModel = resolver.Resolve<SettingsModel>();
                var audioPlayer = new AudioPlayer(musicVolume: settingsModel.MusicVolume.Select(x => x / 10.0f), 
                    sfxVolume: settingsModel.SfxVolume.Select(x => x / 10.0f), monoHook);
                _loadingScreen.OverlayFillProgress.Subscribe(x => audioPlayer.VolumeMultiplier.Value = 1 - x);
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

        private static void SelectLanguageByIndex(int index)
        {
            if (index >= 0 && index < LocalizationSettings.AvailableLocales.Locales.Count)
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            else
                Debug.LogWarning($"Unknown language index: {index}");
        }
    }
}