using UnityEngine;

namespace LostKaiju.Game.World.Player.Behaviour.StateBinders
{
    public class FiniteStatesBinder : MonoBehaviour
    {
        [SerializeField] private FiniteStateBinderTuple[] _binders;
    }

    public struct FiniteStateBinderTuple
    {
        public FiniteStateBinderTags Tag;
        public FiniteStateBinderSO BinderSO;
    }
}
