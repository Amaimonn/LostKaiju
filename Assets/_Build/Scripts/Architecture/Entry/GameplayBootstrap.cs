using UnityEngine;
using Unity.Cinemachine;
using R3;

using LostKaiju.Player.View;
using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitPosition;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
        {
            Debug.Log("Gameplay booted");
            var exitGameplaySignal = new Subject<GameplayExitContext>();
            var playerPrefab = gameplayEnterContext.PlayerBinderSO.Prefab;
            var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            Debug.Log("player instantiated");
            _cinemachineCamera.Follow = player.transform;
            return exitGameplaySignal;
        }
    }
}
