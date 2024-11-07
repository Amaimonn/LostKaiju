using UnityEngine;
using R3;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField] private Transform _playerInitPosition;
    [SerializeField] private PlayerBinder _playerBinder;

    public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
    {
        var exitGameplaySignal = new Subject<GameplayExitContext>();
        var player = Instantiate(_playerBinder, _playerInitPosition.position, Quaternion.identity);
        return exitGameplaySignal;
    }
}
