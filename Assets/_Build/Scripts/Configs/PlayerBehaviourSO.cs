using UnityEngine;

using Assets._Build.Scripts.Player.Data;
using Assets._Build.Scripts.Player.Behaviour;

namespace Assets._Build.Scripts.Configs
{
    [CreateAssetMenu(fileName = "PlayerBehaviourSO", menuName = "Scriptable Objects/PlayerBehaviourSO")]
    public class PlayerBehaviourSO : CharacterBehaviourSO
    {
        public override CharacterBehaviour GetModel() => new PlayerBehaviour(_BaseCharacterInputData);

        [SerializeField] private PlayerControlsData _BaseCharacterInputData;
    }
}