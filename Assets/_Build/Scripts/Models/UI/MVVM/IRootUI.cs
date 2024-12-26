using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Models.UI.MVVM
{
    public interface IRootUI
    {
        public void Attach(VisualElement visualElement);
        public void Attach(GameObject gameObjectUI);
    }
}
