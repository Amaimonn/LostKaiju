using UnityEngine;
using R3;

using LostKaiju.Architecture.Entry.Context;

namespace LostKaiju.Architecture.Entry
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
