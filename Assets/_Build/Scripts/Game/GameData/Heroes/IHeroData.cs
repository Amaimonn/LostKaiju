using UnityEngine;

namespace LostKaiju.Game.GameData.Heroes
{
    public interface IHeroData
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Color Tint { get; }
        public Sprite PreviewSprite { get; }
    }
}