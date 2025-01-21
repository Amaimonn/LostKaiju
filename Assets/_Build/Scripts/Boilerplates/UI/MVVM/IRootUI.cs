using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Boilerplates.UI.MVVM
{
    public interface IRootUI
    {
        public void Attach(VisualElement visualElement);

        public void Attach(GameObject gameObjectUI);

        public void Detach(VisualElement visualElement);
        
        public void Detach(GameObject gameObjectUI);
    }
}
