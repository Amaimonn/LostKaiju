using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    [UxmlElement]
    public partial class PreSizeFollower : VisualElement
    {
        [UxmlAttribute] public bool FollowWidth;
        [UxmlAttribute] public bool FollowHeight;
        private VisualElement _grandParent;

        public PreSizeFollower()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometry);
        }

        // private void OnAttachToPanel(AttachToPanelEvent evt)
        // {
        //     _grandParent = parent;
        //     if (_grandParent != null)
        //     {
        //         if (FollowWidth && FollowHeight )
        //             _grandParent.RegisterCallback<GeometryChangedEvent>(e => OnTargetSizeChanged());
        //         else if (FollowWidth)
        //             _grandParent.RegisterCallback<GeometryChangedEvent>(e => OnTargetWidthChanged());
        //         else if (FollowHeight)
        //             _grandParent.RegisterCallback<GeometryChangedEvent>(e => OnTargetHeightChanged());
        //     }
        // }

        private void OnGeometry(GeometryChangedEvent evt)
        {
            _grandParent = parent?.parent;
            if (_grandParent != null)
            {
                if (FollowWidth && FollowHeight )
                    OnTargetSizeChanged();
                else if (FollowWidth)
                    OnTargetWidthChanged();
                else if (FollowHeight)
                    OnTargetHeightChanged();
            }
        }

        private void OnTargetSizeChanged()
        {
            style.width = _grandParent.contentRect.width;
            style.height = _grandParent.contentRect.height;
        }

        private void OnTargetWidthChanged()
        {
            style.width = _grandParent.contentRect.width;
        }

        private void OnTargetHeightChanged()
        {
            style.height = _grandParent.contentRect.height;
        }
    }
}
