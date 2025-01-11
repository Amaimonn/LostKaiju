using System;
using System.Linq;
using UnityEngine;
using R3;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Models.Locator;
using LostKaiju.Models.UI.MVVM;
using LostKaiju.Gameplay.UI.MVVM.Hub;
using LostKaiju.Gameplay.GameData.Missions;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class HubBootstrap : MonoBehaviour
    {
        [SerializeField] private HubView _hubViewPrefab;
        [SerializeField] private string _playerConfigName; // choose your player character
        [SerializeField] private string _missionsConfigPath;

        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubView = Instantiate(_hubViewPrefab);
            var uiRootBinder = ServiceLocator.Current.Get<IRootUIBinder>();

            var exitSignal = new Subject<Unit>();
            var gameplayEnterContext = new GameplayEnterContext(Scenes.GAMEPLAY)
            {
                LevelSceneName = "Level_1_1", // level loading example
                PlayerConfigPath = $"Gameplay/PlayerConfigs/{_playerConfigName}",
            };
            var hubExitContext = new HubExitContext(gameplayEnterContext);
            var hubExitSignal = exitSignal.Select(_ => hubExitContext); // send to UI
            var missionsModelFactory = new Func<MissionsModel>(() => MissionsModelFactory(gameplayEnterContext));
            var hubViewModel = new HubViewModel(exitSignal, missionsModelFactory);

            hubView.Bind(hubViewModel);
            uiRootBinder.SetView(hubView);

            return hubExitSignal;
        }

        private MissionsModel MissionsModelFactory(GameplayEnterContext gameplayEnterContext)
        {
            var missionsArraySO = Resources.Load<MissionsArraySO>(_missionsConfigPath);
            var missions = missionsArraySO.Missions.Select(x => x.Mission);
            var selectedMission = missions.First();
            var missionsModel = new MissionsModel(missions, selectedMission);

            missionsModel.SelectedMission.Subscribe(x => {
                gameplayEnterContext.LevelSceneName = x.SceneName;
                Debug.Log($"r3: Scene in signal: {x.SceneName}");
            });

            return missionsModel;
        }
    }
}
