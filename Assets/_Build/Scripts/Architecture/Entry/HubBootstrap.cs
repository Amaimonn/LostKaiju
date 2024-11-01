using R3;

public class HubBootstrap
{
    public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
    {
        var hubExitSignal = new Subject<HubExitContext>();
        // var levelId = "Gameplay";
        // var gameplayEnterContext = new GameplayEnterContext(levelId);
        // var hubExitContext = new HubExitContext(gameplayEnterContext);
        // var hubExitToGameplaySignal = hubExitSignal.Select(_ => hubExitContext);
        return hubExitSignal;
    }
}
