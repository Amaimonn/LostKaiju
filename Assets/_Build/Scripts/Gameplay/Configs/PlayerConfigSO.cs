using UnityEngine;

using LostKaiju.Gameplay.Creatures.Views;
using LostKaiju.Gameplay.Player.Data.Configs;

namespace LostKaiju.Gameplay.Configs 
{
    [CreateAssetMenu(fileName = "PlayerConfigSO", menuName = "Scriptable Objects/Player Config SO")]
    public class PlayerConfigSO : ScriptableObject, IPlayerConfig
    {
        [field: SerializeField] public CreatureBinder CreatureBinder { get; private set;}
        public IPlayerData PlayerData => _playerDataSO;

        [SerializeField] private PlayerDataSO _playerDataSO;
    }
}
