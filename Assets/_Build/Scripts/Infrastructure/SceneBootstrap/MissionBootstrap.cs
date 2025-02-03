using UnityEngine;
using Unity.Cinemachine;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.UI.MVVM.Gameplay.PlayerCreature;
using LostKaiju.Game.Player;
using LostKaiju.Game.Player.Behaviour;
using LostKaiju.Game.Player.Data.Configs;
using LostKaiju.Boilerplates.Locator;
using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Player.Data.Indicators;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MissionBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private string _playerIndicatorsViewPrefabPath = "UI/Gameplay/PlayerIndicatorsView";

        public void Boot(GameplayEnterContext gameplayEnterContext)
        {
            var playerConfigPath = gameplayEnterContext.PlayerConfigPath;
            var playerConfigSO = Resources.Load<PlayerConfigSO>(playerConfigPath);
            var playerPrefab = playerConfigSO.CreatureBinder;
            var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            Debug.Log("player instantiated");

            var playerData = playerConfigSO.PlayerData;
            var healthState = new HealthState(playerData.PlayerDefenceData.MaxHealth);
            var healthModel = new HealthModel(healthState);
            var playerIndicatorsViewModel = new PlayerIndicatorsViewModel(healthModel);
            var serviceLocator = ServiceLocator.Instance;
            var rootUIBinder = serviceLocator.Get<IRootUIBinder>();
            var playerIndicatorsViewPrefab = Resources.Load<PlayerIndicatorsView>(_playerIndicatorsViewPrefabPath);
            var playerIndicatorsView = Instantiate(playerIndicatorsViewPrefab);
            playerIndicatorsView.Bind(playerIndicatorsViewModel);
            rootUIBinder.AddView(playerIndicatorsView); // TODO: UI cleaning logic

            var playerInputPresenter = new PlayerInputPresenter(playerData.PlayerControlsData);
            var playerDefencePresenter = new PlayerDefencePresenter(healthModel, playerData.PlayerDefenceData);
            var playerRootPresenter = new PlayerRootPresenter(playerInputPresenter, playerDefencePresenter);
            playerRootPresenter.Bind(player);
            Debug.Log("player root presenter binded");
            // rootUIBinder.AddView()

            _cinemachineCamera.Follow = player.transform;
        }
    }
}
