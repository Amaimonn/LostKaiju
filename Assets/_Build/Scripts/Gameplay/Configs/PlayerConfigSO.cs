using UnityEngine;

using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Player.Data.Configs;

namespace LostKaiju.Game.Configs 
{
    [CreateAssetMenu(fileName = "PlayerConfigSO", menuName = "Scriptable Objects/Player Config SO")]
    public class PlayerConfigSO : ScriptableObject, IPlayerConfig
    {
        [field: SerializeField] public CreatureBinder CreatureBinder { get; private set;}
        public IPlayerData PlayerData => _playerDataSO;

        [SerializeField] private PlayerDataSO _playerDataSO;
    }
}
