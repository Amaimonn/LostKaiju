using UnityEngine;
using R3;

public class MainMenuBootstrap : MonoBehaviour
{
    public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
    {
        var mainMenuExitSignal = new Subject<MainMenuExitContext>();
        return mainMenuExitSignal;
    }
}
