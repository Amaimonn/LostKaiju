using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Boilerplates.UI.MVVM
{
    public abstract class HybridView<T> : View<T> where T : IViewModel
    {
        [SerializeField] protected VisualTreeAsset _visualTreeAsset;

        protected abstract CanvasOrder Order { get; }
        protected VisualElement Root { get; private set; }

        public sealed override void Attach(IRootUI rootUI)
        {
            rootUI.Attach(gameObject, Order);
            rootUI.Attach(Root);
        }

        public sealed override void Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(Root);
            rootUI.Detach(gameObject);
        }
    }
}