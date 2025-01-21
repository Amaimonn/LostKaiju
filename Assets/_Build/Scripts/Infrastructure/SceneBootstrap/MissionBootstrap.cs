using UnityEngine;
using Unity.Cinemachine;

using LostKaiju.Infrastructure.SceneBootstrap.Context;
using LostKaiju.Game.Player;
using LostKaiju.Game.Configs;
using LostKaiju.Game.Player.Behaviour;

namespace LostKaiju.Infrastructure.SceneBootstrap
{
    public class MissionBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public void Boot(GameplayEnterContext gameplayEnterContext)
        {
            var playerConfigPath = gameplayEnterContext.PlayerConfigPath;
            var playerConfigSO = Resources.Load<PlayerConfigSO>(playerConfigPath);
            var playerPrefab = playerConfigSO.CreatureBinder;
            var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            Debug.Log("player instantiated");

            var playerData = playerConfigSO.PlayerData;
            var playerInputPresenter = new PlayerInputPresenter(playerData.PlayerControlsData);
            var playerDefencePresenter = new PlayerDefencePresenter(playerData.PlayerDefenceData);
            var playerRootPresenter = new PlayerRootPresenter(playerInputPresenter, playerDefencePresenter);
            playerRootPresenter.Bind(player);
            Debug.Log("player root presenter binded");

            _cinemachineCamera.Follow = player.transform;
        }
    }
}
