using UnityEngine;
using R3;

using Assets._Build.Scripts.Architecture.Entry.Context;

namespace Assets._Build.Scripts.Architecture.Entry
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        public Observable<MainMenuExitContext> Boot(MainMenuEnterContext mainMenuEnterContext = null)
        {
            var mainMenuExitSignal = new Subject<MainMenuExitContext>();
            return mainMenuExitSignal;
        }
    }
}
