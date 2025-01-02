using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CharacterPreview : VisualElement
{
    [UxmlAttribute] public GameObject _character;
}
