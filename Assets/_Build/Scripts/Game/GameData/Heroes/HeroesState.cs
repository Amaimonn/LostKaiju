using System;
using UnityEngine;

namespace LostKaiju.Game.GameData.Heroes
{
    [Serializable]
    public class HeroesState : IVersioned, ICopyable<HeroesState>
    {
        public int Version { get => _version; set => _version = value; }
        [SerializeField] private int _version = 1;

        public string SelectedHeroId;

        public HeroesState Copy()
        {
            var copy = (HeroesState)MemberwiseClone();
            return copy;
        }
    }
}