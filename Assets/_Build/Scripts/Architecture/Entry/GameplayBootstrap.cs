using UnityEngine;
using Unity.Cinemachine;
using R3;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
{
    /// <summary>
    /// General Bootstrap for gameplay levels
    /// </summary>
    public class GameplayBootstrap : MonoBehaviour
    {
        // [SerializeField] private Transform _playerInitPosition;
        // [SerializeField] private CinemachineCamera _cinemachineCamera;

        public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
        {
            var exitGameplaySignal = new Subject<GameplayExitContext>();
            // var playerPrefab = gameplayEnterContext.PlayerConfig.PlayerBinder;
            // var player = Instantiate(playerPrefab, _playerInitPosition.position, Quaternion.identity);
            // Debug.Log("player instantiated");
            // _cinemachineCamera.Follow = player.transform;

            return exitGameplaySignal;
        }
    }
}
