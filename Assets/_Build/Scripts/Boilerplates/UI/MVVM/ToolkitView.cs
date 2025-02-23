using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Boilerplates.UI.MVVM
{
    public abstract class ToolkitView<T> : View<T> where T : IViewModel
    {
        [SerializeField] protected VisualTreeAsset _visualTreeAsset;

        protected VisualElement Root { get; private set; }

#region View<T>
        public sealed override void Attach(IRootUI rootUI)
        {
            rootUI.Attach(Root);
        }

        public sealed override void Detach(IRootUI rootUI)
        {
            rootUI.Detach(Root);
            Dispose();
            Destroy(gameObject);
        }
#endregion

#region MonoBehaviour
        private void Awake()
        {
            Root = _visualTreeAsset.CloneTree();
            var styles = Root.style;

            styles.position = Position.Absolute;
            styles.left = 0;
            styles.right = 0;
            styles.top = 0;
            styles.bottom = 0;

            OnAwake();
        }
#endregion

        protected virtual void OnAwake()
        {
        }
    }
}