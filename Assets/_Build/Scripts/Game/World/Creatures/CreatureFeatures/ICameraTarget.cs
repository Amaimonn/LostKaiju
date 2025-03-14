using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Creatures.Features
{
    public interface ICameraTarget : ICreatureFeature
    {
        public Transform TargetTransform { get; }
    }
}
