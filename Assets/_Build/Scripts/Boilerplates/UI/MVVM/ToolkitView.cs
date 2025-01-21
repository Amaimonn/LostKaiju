using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Boilerplates.UI.MVVM;

public abstract class ToolkitView<T> : View<T> where T : IViewModel
{
    [SerializeField] protected VisualTreeAsset _visualTreeAsset;
    protected VisualElement _root;
    
#region View<T>
    public sealed override void Attach(IRootUI rootUI)
    {
        rootUI.Attach(_root);
    }

    public sealed override void Detach(IRootUI rootUI)
    {
        rootUI.Detach(_root);
        Dispose();
        Destroy(gameObject);
    }
#endregion

#region MonoBehaviour
    private void Awake()
    {
        _root = _visualTreeAsset.CloneTree();
        var lenght = Length.Percent(100);
        
        _root.style.position = Position.Absolute;
        _root.style.width = lenght;
        _root.style.height = lenght;
        OnAwake();
    }
#endregion

    protected virtual void OnAwake()
    {
    }
}