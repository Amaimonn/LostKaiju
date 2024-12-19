using LostKaiju.UI.MVVM;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ToolkitView<T> : View<T> where T : IViewModel
{
    [SerializeField] protected VisualTreeAsset _visualTreeAsset;
    protected VisualElement _root;

    public sealed override void Attach(IRootUI rootUI)
    {
        _root = _visualTreeAsset.CloneTree();
        var lenght = Length.Percent(100);
        
        _root.style.width = lenght;
        _root.style.height = lenght;
        // _root.style.position = Position.Absolute;
        
        rootUI.Attach(_root);
    }
}