using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Boilerplates.UI.MVVM
{
    public class RootUIBinder : MonoBehaviour, IRootUIBinder, IRootUI
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Transform _canvasUiRoot;
        [SerializeField] private VisualElement _visualElementRoot;
        [SerializeField] private string _visualElementRootName;

#region IRootUIBinder
        public void SetView(View view)
        {
            ClearViews();
            AddView(view);
        }

        public void SetViews(IEnumerable<View> views)
        {
            ClearViews();
            AddViews(views);
        }

        public void SetViews(params View[] views)
        {
            ClearViews();
            AddViews(views);
        }

        public void AddView(View view)
        {
            view.Attach(this);
        }

        public void AddViews(params View[] views)
        {
            foreach (var view in views)
            {
                view.Attach(this);
            }
        }

        public void AddViews(IEnumerable<View> views)
        {
            foreach (var view in views)
            {
                view.Attach(this);
            }
        }

        public void ClearView(View view)
        {
            if (view != null)
                view.Detach(this);
        }

        public void ClearViews()
        {
            ClearToolkitViews();
            ClearCanvasViews();
        }
#endregion

        private void ClearCanvasViews()
        {
            int childrenAmount = _canvasUiRoot.childCount;

            for (int i = childrenAmount - 1; i >= 0; i--)
            {
                Destroy(_canvasUiRoot.GetChild(i).gameObject);
            }
        }

        private void ClearToolkitViews()
        {
            _visualElementRoot.Clear();
        }

#region MonoBehaviour
        private void Awake()
        {
            _visualElementRoot = _document.rootVisualElement.Q(name: _visualElementRootName);
        }
#endregion

#region IRootUI
/// <summary>
/// This is only used by UI Toolkit Views in terms of implementation Visitor pattern. 
/// For the scene UI binding use SetViews or AddViews method instead.
/// </summary>
/// <param name="visualElement"></param>
        public void Attach(VisualElement visualElement)
        {
            _visualElementRoot.Add(visualElement);
        }

/// <summary>
/// This is only used by Canvas Views in terms of implementation Visitor pattern. 
/// For the scene UI binding use SetViews or AddViews method instead.
/// </summary>
/// <param name="gameObjectUI"></param>

        public void Attach(GameObject gameObjectUI)
        {
            gameObjectUI.transform.SetParent(_canvasUiRoot, false);
        }

        public void Detach(VisualElement visualElement)
        {
            _visualElementRoot.Remove(visualElement);
        }

        public void Detach(GameObject gameObjectUI)
        {
            Destroy(gameObjectUI);
        }
#endregion
    }
}
