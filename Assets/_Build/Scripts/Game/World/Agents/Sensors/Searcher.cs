using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostKaiju.Game.World.Agents.Sensors
{
    public abstract class Searcher<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected SensorParameters _parameters;

        protected Dictionary<T, Coroutine> _searchCoroutines = new();
        protected WaitForSeconds _wait;

        protected void Awake()
        {
            _wait = new WaitForSeconds(_parameters.RayCooldown);
        }

        protected abstract void OnTargetFound(T target);
        protected abstract void OnTargetLost(T target);

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var target))
            {
                if (!_searchCoroutines.ContainsKey(target))
                    _searchCoroutines[target] = StartCoroutine(CheckTargetForVisibility(target));
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<T>(out var exitingTarget))
            {
                if (_searchCoroutines.ContainsKey(exitingTarget))
                {
                    StopCoroutine(_searchCoroutines[exitingTarget]);
                    _searchCoroutines.Remove(exitingTarget);
                    OnTargetLost(exitingTarget);
                }
            }
        }

        protected virtual IEnumerator CheckTargetForVisibility(T target)
        {
            while (true)
            { 
                var rayDirection = target.transform.position + _parameters.RayOffset
                    + _parameters.RandomOffset * Random.Range(-1.0f, 1.0f)
                    - _parameters.RayOrigin.position;
                    
                var hit = Physics2D.Raycast(_parameters.RayOrigin.position, rayDirection, 
                    _parameters.RayDistance, _parameters.RayMask);

                if (hit)
                {
                    if (hit.collider.TryGetComponent<T>(out var visible))
                    {
                        if (visible.Equals(target))
                        {
                            OnTargetFound(target);
                            Debug.DrawLine(_parameters.RayOrigin.position, hit.point, Color.red, 1f);
                        }
                    }
                    else
                    {
                        OnTargetLost(target);
                    }
                }
                else
                {
                    OnTargetLost(target);
                }

                yield return _wait;
            }
        }
    }
}