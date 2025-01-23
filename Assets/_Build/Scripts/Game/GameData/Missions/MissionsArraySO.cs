using UnityEngine;

namespace LostKaiju.Game.GameData.Missions
{
    [CreateAssetMenu(fileName = "MissionsArraySO", menuName = "Scriptable Objects/MissionsArraySO")]
    public class MissionsArraySO : ScriptableObject
    {
        [field: SerializeField] public MissionSO[] Missions { get; private set; }
    }
}
