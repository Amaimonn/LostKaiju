using UnityEngine;
using R3;

public class GameplayBootstrap : MonoBehaviour
{
    public Observable<GameplayExitContext> Boot(GameplayEnterContext gameplayEnterContext)
    {
        var exitGameplaySignal = new Subject<GameplayExitContext>();
        return exitGameplaySignal;
    }
}
