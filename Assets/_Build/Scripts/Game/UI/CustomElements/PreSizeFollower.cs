using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    public class PreSizeFollower : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<PreSizeFollower, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlBoolAttributeDescription _followWidth = new UxmlBoolAttributeDescription { name = "follow-width", defaultValue = false };
            private readonly UxmlBoolAttributeDescription _followHeight = new UxmlBoolAttributeDescription { name = "follow-height", defaultValue = false };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var preSizeFollower = ve as PreSizeFollower;

                preSizeFollower.FollowWidth = _followWidth.GetValueFromBag(bag, cc);
                preSizeFollower.FollowHeight = _followHeight.GetValueFromBag(bag, cc);
            }
        }

        public bool FollowWidth { get; set; } = false;
        public bool FollowHeight { get; set; } = false;

        private VisualElement _grandParent;

        public PreSizeFollower()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometry);
        }

        private void OnGeometry(GeometryChangedEvent evt)
        {
            _grandParent = parent?.parent;
            if (_grandParent != null)
            {
                if (FollowWidth && FollowHeight)
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
