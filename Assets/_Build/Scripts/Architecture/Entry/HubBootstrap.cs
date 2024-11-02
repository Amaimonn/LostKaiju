using UnityEngine;
using R3;

public class HubBootstrap : MonoBehaviour
{
    public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
    {
        var hubExitSignal = new Subject<HubExitContext>();
        return hubExitSignal;
    }
}
