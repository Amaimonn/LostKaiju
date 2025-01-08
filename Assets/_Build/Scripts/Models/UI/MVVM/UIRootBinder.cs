using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using LostKaiju.Models.Locator;

namespace LostKaiju.Models.UI.MVVM
{
    public class UIRootBinder : MonoBehaviour, IRootUI, IService
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Transform _canvasUiRoot;
        [SerializeField] private VisualElement _visualElementRoot;
        [SerializeField] private string _visualElementRootName;

        public void SetViews(View view)
        {
            ClearViews();
            AddViews(view);
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

        public void AddViews(View view)
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

#region MonoBehaviour
        private void Awake()
        {
            _visualElementRoot = _document.rootVisualElement.Q(name: _visualElementRootName);
        }
#endregion

#region IRootUI
/// <summary>
/// This is only used by UI Tookkit Views in terms of implementation Visiter pattern. 
/// For the scene UI binding use SetViews or AddViews method instead.
/// </summary>
/// <param name="visualElement"></param>
        public void Attach(VisualElement visualElement)
        {
            _visualElementRoot.Add(visualElement);
        }

/// <summary>
/// This is only used by Canvas Views in terms of implementation Visiter pattern. 
/// For the scene UI binding use SetViews or AddViews method instead.
/// </summary>
/// <param name="gameObjectUI"></param>

        public void Attach(GameObject gameObjectUI)
        {
            gameObjectUI.transform.SetParent(_canvasUiRoot, false);
        }
#endregion

        public void ClearViews()
        {
            ClearToolkitViews();
            ClearCanvasViews();
        }

        private void ClearCanvasViews()
        {
            int childrenAmount = _canvasUiRoot.childCount;

            for (int i = childrenAmount-1; i >=0; i--)
            {
                Destroy(_canvasUiRoot.GetChild(i).gameObject);
            }
        }

        private void ClearToolkitViews()
        {
            _visualElementRoot.Clear();
        }
    }
}
