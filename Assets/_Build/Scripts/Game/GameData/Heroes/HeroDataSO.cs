using UnityEngine;

namespace LostKaiju.Game.GameData.Heroes
{
    [CreateAssetMenu(fileName = "HeroDataSO", menuName = "Scriptable Objects/Heroes/HeroDataSO")]
    public class HeroDataSO : ScriptableObject, IHeroData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Color Tint { get; private set; }
        [field: SerializeField] public Sprite PreviewSprite { get; private set; }
    }
}
