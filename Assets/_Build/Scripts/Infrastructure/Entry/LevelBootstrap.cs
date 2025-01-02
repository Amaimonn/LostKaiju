using UnityEngine;
using Unity.Cinemachine;

using LostKaiju.Infrastructure.Entry.Context;

namespace LostKaiju.Infrastructure.Entry
{
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public void Boot(GameplayEnterContext gameplayEnterContext)
        {
            var playerPrefab = gameplayEnterContext.PlayerConfig.CreatureBinder;
            var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            Debug.Log("player instantiated");
            _cinemachineCamera.Follow = player.transform;
        }
    }
}
