using UnityEngine;
using R3;

using Assets._Build.Scripts.Architecture.Entry.Context;

namespace Assets._Build.Scripts.Architecture.Entry
{
    public class HubBootstrap : MonoBehaviour
    {
        public Observable<HubExitContext> Boot(HubEnterContext hubEnterContext)
        {
            var hubExitSignal = new Subject<HubExitContext>();
            return hubExitSignal;
        }
    }
}
