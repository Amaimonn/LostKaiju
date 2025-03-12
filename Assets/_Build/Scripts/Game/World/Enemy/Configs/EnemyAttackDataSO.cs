using UnityEngine;

namespace LostKaiju.Game.World.Enemy.Configs
{
    [CreateAssetMenu(fileName = "EnemyAttackDataSO", menuName = "Scriptable Objects/Enemy/EnemyAttackDataSO")]
    public class EnemyAttackDataSO : ScriptableObject
    {
        [field: SerializeField] public float AttackDelay { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
    }
}