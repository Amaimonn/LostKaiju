using UnityEngine;

public class FiniteStatesBinder : MonoBehaviour
{
    [SerializeField] private FiniteStateBinderTuple[] _binders;
}

public struct FiniteStateBinderTuple
{
    public FiniteStateBinderTags Tag;
    public FiniteStateBinderSO BinderSO;
}