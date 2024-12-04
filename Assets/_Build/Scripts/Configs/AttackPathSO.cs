using System.Collections;
using UnityEngine;

using LostKaiju.Entities.Combat.AttackSystem;

namespace LostKaiju.Configs 
{
    [CreateAssetMenu(fileName = "AttackPathSO", menuName = "Scriptable Objects/AttackPathSO")]
    public class AttackPathSO : ScriptableObject, IAttackPath
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _xAreaSize;
        [SerializeField] private AnimationCurve _yAreaSize;
        [SerializeField] private AnimationCurve _xPath;
        [SerializeField] private AnimationCurve _yPath;
        [SerializeField] private float _xStartPosition;
        [SerializeField] private float _yStartPosition;
        
        public IEnumerator Process(Collider2D collider)
        {
            var attackTransform = collider.gameObject.transform;

            attackTransform.localPosition = new Vector2(_xStartPosition, _yStartPosition);
            
            float progress = 0;

            while (progress < 1)
            {
                attackTransform.localScale = new Vector2(_xAreaSize.Evaluate(progress), _yAreaSize.Evaluate(progress));
                attackTransform.localPosition = new Vector2(_xPath.Evaluate(progress), _yPath.Evaluate(progress));
                progress += Time.deltaTime / _duration;
                yield return null;
            }
        }
    }
}