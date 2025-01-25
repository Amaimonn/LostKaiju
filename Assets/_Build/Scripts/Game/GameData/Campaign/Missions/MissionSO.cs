using UnityEngine;

namespace LostKaiju.Game.GameData.Campaign.Missions
{
    [CreateAssetMenu(fileName = "MissionSO", menuName = "Scriptable Objects/MissionSO")]
    public class MissionSO : ScriptableObject
    {
        [field: SerializeField] public MissionData Mission { get; private set; }
    }
}
