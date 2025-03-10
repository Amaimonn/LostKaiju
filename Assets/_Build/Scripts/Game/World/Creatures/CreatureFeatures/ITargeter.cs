using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public interface ITargeter : ICreatureFeature
    {
        public abstract bool IsTargeting { get; }
        public abstract void SetTarget(Transform targetTransform);
        public abstract Vector3 GetTargetPosition();
    }
}