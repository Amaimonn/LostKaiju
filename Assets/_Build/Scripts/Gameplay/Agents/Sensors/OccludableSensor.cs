using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostKaiju.Gameplay.Agents.Sensors
{
    public class OccludableSensor<T> : Sensor<T> where T : Component
    {
        [SerializeField] protected Transform _rayOrigin;
        [SerializeField] protected float _rayDistance;
        [SerializeField] protected Transform _rayDirection;
        [SerializeField] protected float _rayCooldown = 0.1f;
        [SerializeField] protected LayerMask _rayMask;

        protected Dictionary<T, Coroutine> _searchCoroutines = new();
        protected WaitForSeconds _wait;

        protected void Awake()
        {
            _wait = new WaitForSeconds(_rayCooldown);
        }

        protected override void  OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var target))
            {
                _searchCoroutines.Add(target, StartCoroutine(CheckTargetForVisibility(target)));
            }
        }

        protected override void  OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var exitingTarget))
            {
                if (_searchCoroutines.ContainsKey(exitingTarget))
                {
                    StopCoroutine(_searchCoroutines[exitingTarget]);
                    _searchCoroutines.Remove(exitingTarget);
                    if (_detected.Value != null && _detected.Value.Equals(exitingTarget))
                    {
                        _detected.Value = default;
                    }
                }
            }
        }    
        
        protected virtual IEnumerator CheckTargetForVisibility(T target)
        {
            while (true)
            {
                if (_detected.Value == null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(_rayOrigin.position, target.transform.position - _rayOrigin.position, _rayDistance, _rayMask);
                    if (hit)
                    {
                        if (hit.collider.TryGetComponent<T>(out var visible))
                        {
                            if (visible.Equals(target))
                            {
                                _detected.Value = visible;
                                Debug.DrawLine(_rayOrigin.position, hit.point, Color.red, 1f);
                            }
                        }
                        else
                        {
                            _detected.Value = default;
                        }
                    }
                }
                yield return new WaitForSeconds(_rayCooldown);
            }
        }
    }
}
