using UnityEngine;
using R3;

namespace LostKaiju.Game.World.Creatures.Features
{
    public class GroundedPusher : Pusher
    {
        public override void Push(Vector2 forceOrigin, float force)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            var directionVector = (Vector2)_ownOrigin.position - forceOrigin;
            if (directionVector.y < 0)
                directionVector.y = 0;
            _rigidbody.AddForce(force * directionVector.normalized, ForceMode2D.Impulse);
            _onPushed.OnNext(Unit.Default);
        }
    }
}