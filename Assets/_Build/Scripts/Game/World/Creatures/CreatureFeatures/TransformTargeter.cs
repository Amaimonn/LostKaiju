using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public class Targeter : MonoBehaviour, ITargeter
    {
        public bool IsTargeting => _targetTransform != null;
        private Transform _targetTransform;

        public Vector3 GetTargetPosition()
        {
            return _targetTransform.position;
        }

        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }
    }
} 
