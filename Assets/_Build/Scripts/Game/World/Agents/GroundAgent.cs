using UnityEngine;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Agents
{
    public class GroundAgent : Agent
    {
        public float StopDistance => _stopDistance;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Flipper _flipper;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _stopDistance;

        private float _destinationPointDistance;
        private Vector3 _vectorToDestinationPoint;
        private bool IsWithinStopDistance => _destinationPointDistance <= _stopDistance;

        public override void SetDestination(Vector3 point)
        {
            Destination = point;
            CalculateDestinationParameters();
            if (!IsWithinStopDistance)
            {
                _isStopped.Value = false;
                _flipper.LookRight(_vectorToDestinationPoint.x > 0);
            }
        }

        public override void LookAt(Vector3 point)
        {
            _flipper.LookRight(point.x > transform.position.x);
        }

        public override void Stop()
        {
            _isStopped.Value = true;
        }

        private void FixedUpdate()
        {
            if (!_isStopped.Value)
                GoToTheDestinationPoint();
        }

        private void GoToTheDestinationPoint()
        {
            CalculateDestinationParameters();
            if (IsWithinStopDistance)
            {
                _isStopped.Value = true;
                return;
            }
            else
            {
                _rigidbody.linearVelocityX = _speed * Mathf.Sign(_vectorToDestinationPoint.x);
            }
        }

        private void CalculateDestinationParameters()
        {
            _vectorToDestinationPoint = Destination - transform.position;
            _destinationPointDistance = Mathf.Abs(_vectorToDestinationPoint.x);
        }
    }
}
