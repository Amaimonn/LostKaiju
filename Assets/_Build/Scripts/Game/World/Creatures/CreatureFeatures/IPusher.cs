using R3;
using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public interface IPusher : ICreatureFeature
    {
        public Observable<Unit> OnPushed { get; }
        public void Push(Vector2 origin, float force);
    }
}