using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    [CreateAssetMenu(fileName = "ForceAttackDataSO", menuName = "Scriptable Objects/ForceAttackDataSO")]
    public class ForceAttackDataSO : ScriptableObject
    {
        [field: SerializeField] public float Force { get; private set; }
    }
}