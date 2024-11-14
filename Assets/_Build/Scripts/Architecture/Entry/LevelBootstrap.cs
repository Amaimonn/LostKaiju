using UnityEngine;
using Unity.Cinemachine;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    public class LevelBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public void Boot(GameplayEnterContext gameplayEnterContext)
        {
            var playerPrefab = gameplayEnterContext.PlayerConfig.PlayerBinder;
            var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            Debug.Log("player instantiated");
            _cinemachineCamera.Follow = player.transform;
        }
    }
}
