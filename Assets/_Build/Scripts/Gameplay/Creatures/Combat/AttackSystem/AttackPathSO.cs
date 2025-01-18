using System.Collections;
using UnityEngine;

namespace LostKaiju.Gameplay.Creatures.Combat.AttackSystem
{
    [CreateAssetMenu(fileName = "AttackPathSO", menuName = "Scriptable Objects/AttackPathSO")]
    public class AttackPathSO : ScriptableObject, IAttackPath
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _xScaleCurve;
        [SerializeField] private AnimationCurve _yScaleCurve;
        [SerializeField] private AnimationCurve _xPath;
        [SerializeField] private AnimationCurve _yPath;
        [SerializeField] private float _xStartPosition;
        [SerializeField] private float _yStartPosition;
        
        public IEnumerator Process(Transform attackTransform)
        {
            attackTransform.localPosition = new Vector2(_xStartPosition, _yStartPosition);
            
            float progress = 0;

            while (progress < 1)
            {
                EvaluatePathValues(attackTransform, progress);
                progress += Time.deltaTime / _duration;
                yield return null;
            }
            
            EvaluatePathValues(attackTransform, 1);
            yield return null;
        }

        private void EvaluatePathValues(Transform attackTransform, float progress)
        {
            attackTransform.localScale = new Vector2(_xScaleCurve.Evaluate(progress), _yScaleCurve.Evaluate(progress));
            attackTransform.localPosition = new Vector2(_xPath.Evaluate(progress), _yPath.Evaluate(progress));
        }
    }
}