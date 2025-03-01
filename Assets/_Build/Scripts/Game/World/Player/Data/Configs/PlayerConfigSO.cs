using UnityEngine;

using LostKaiju.Game.World.Creatures.Views;

namespace LostKaiju.Game.World.Player.Data.Configs 
{
    [CreateAssetMenu(fileName = "PlayerConfigSO", menuName = "Scriptable Objects/Player Config SO")]
    public class PlayerConfigSO : ScriptableObject, IPlayerConfig
    {
#region IPlayerConfig
        [field: SerializeField] public CreatureBinder CreatureBinder { get; private set;}
        public IPlayerData PlayerData => _playerDataSO;
#endregion

        [SerializeField] private PlayerDataSO _playerDataSO;
    }
}
