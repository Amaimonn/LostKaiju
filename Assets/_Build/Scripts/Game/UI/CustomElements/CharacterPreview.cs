using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    [UxmlElement]
    public partial class CharacterPreview : VisualElement
    {
        [UxmlAttribute] public GameObject _character;
    }
}
