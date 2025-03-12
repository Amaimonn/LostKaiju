using UnityEngine;

namespace LostKaiju.Game.World.Enemy.Configs
{
    [CreateAssetMenu(fileName = "EnemyDefenceDataSO", menuName = "Scriptable Objects/Enemy/EnemyDefenceDataSO")]
    public class EnemyDefenceDataSO : ScriptableObject, IEnemyDefenceData
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
    }
}