using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Architecture.Services;

namespace LostKaiju.UI.MVVM
{
    public class UIRootBinder : MonoBehaviour, IRootUI, IService
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Transform _canvasRoot;
        [SerializeField] private VisualElement _visualElementRoot;
        [SerializeField] private string _visualElementRootName = "UIRoot";

        public void AddView(View view)
        {
            view.Attach(this);
        }

#region MonoBehaviour
        private void Awake()
        {
            _visualElementRoot = _document.rootVisualElement.Q(name: _visualElementRootName);
        }
#endregion

#region IRootUI
        public void Attach(VisualElement visualElement)
        {
            _visualElementRoot.Clear();
            _visualElementRoot.Add(visualElement);
        }

        public void Attach(GameObject gameObjectUI)
        {
            gameObjectUI.transform.SetParent(_canvasRoot);
        }
#endregion
    }
}
