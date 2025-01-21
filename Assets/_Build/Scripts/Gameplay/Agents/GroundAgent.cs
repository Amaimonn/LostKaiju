using UnityEngine;

using LostKaiju.Game.Creatures.Features;

namespace LostKaiju.Game.Agents
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

        private void FixedUpdate()
        {
            if (!_isStopped.Value)
                GoToDestination();
        }

        private void GoToDestination()
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
