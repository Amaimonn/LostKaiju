using UnityEngine;
using Unity.Cinemachine;

using LostKaiju.Infrastructure.Entry.Context;
using LostKaiju.Gameplay.Player;
using LostKaiju.Gameplay.Configs;
using LostKaiju.Gameplay.Player.Behaviour;

namespace LostKaiju.Infrastructure.Entry
{
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public void Boot(GameplayEnterContext gameplayEnterContext)
        {
            var path = gameplayEnterContext.PlayerConfigPath;
            var playerConfigSO = Resources.Load<PlayerConfigSO>(path);
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
