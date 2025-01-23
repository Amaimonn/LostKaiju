using UnityEngine;

namespace LostKaiju.Game.Player.Data.Configs
{
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/Player Data SO")]
    public class PlayerDataSO : ScriptableObject, IPlayerData
    {
        [field: SerializeField] public PlayerControlsData PlayerControlsData { get; private set; }
        [field: SerializeField] public PlayerDefenceData PlayerDefenceData { get; private set; }
    }
}