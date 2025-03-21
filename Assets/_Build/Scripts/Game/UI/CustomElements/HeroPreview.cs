using System;
using UnityEngine;

namespace LostKaiju.Game.UI.CustomElements
{
    public class HeroPreview
    {
        private readonly Transform _heroTransform;
        private GameObject _currentPreview;
        private readonly Action<GameObject> _onShowPreview;
        private readonly Action<GameObject> _onHidePreview;

        public HeroPreview(Transform heroTransform, Action<GameObject> onShowPreview, Action<GameObject> onHidePreview)
        {
            _heroTransform = heroTransform;
            _onShowPreview = onShowPreview;
            _onHidePreview = onHidePreview;
        }

        public void SetPreview(GameObject hero)
        {
            if (_currentPreview != null)
            {
                _onHidePreview(_currentPreview);
            }
            hero.transform.parent = _heroTransform;
            hero.transform.localPosition = Vector3.zero;
            _currentPreview = hero;
            _onShowPreview(_currentPreview);
        }
    }
}
