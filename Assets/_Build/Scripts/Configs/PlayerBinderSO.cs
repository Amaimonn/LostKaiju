using UnityEngine;

using LostKaiju.Player.View;

namespace LostKaiju.Configs 
{
    [CreateAssetMenu(fileName = "PlayerBinderSO", menuName = "Scriptable Objects/PlayerBinderSO")]
    public class PlayerBinderSO : ScriptableObject, IPlayerConfig
    {
        public PlayerBinder PlayerBinder => _playerBinder;

        [SerializeField] private PlayerBinder _playerBinder;
    }
}
