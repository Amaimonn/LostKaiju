using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Combat.AttackSystem
{
    [CreateAssetMenu(fileName = "ShootDataSO", menuName = "Scriptable Objects/ShootDataSO")]
    public class ShootDataSO : ScriptableObject
    {
        [field: SerializeField] public int Damage { get; private set; } = 10;
        [field: SerializeField] public float Speed { get; private set; } = 20;
    }
}