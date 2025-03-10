using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Features
{
    public class Targeter : MonoBehaviour, ITargeter
    {
        public bool IsTargeting => _isTargeting;
        private bool _isTargeting = false;
        private Transform _targetTransform = null;

        public Vector3 GetTargetPosition()
        {
            return _targetTransform.position;
        }

        public void SetTarget(Transform targetTransform)
        {
            if (targetTransform != null)
            {
                _targetTransform = targetTransform;
                _isTargeting = true;
            }
            else
            {
                _targetTransform = null;
                _isTargeting = false;
            }
        }
    }
} 
