using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
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
