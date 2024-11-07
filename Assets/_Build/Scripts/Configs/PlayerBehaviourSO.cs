using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBehaviourSO", menuName = "Scriptable Objects/PlayerBehaviourSO")]
public class PlayerBehaviourSO : CharacterBehaviourSO
{
    public override CharacterBehaviour GetModel() => new PlayerBehaviour(_BaseCharacterInputData);

    [SerializeField] private PlayerControlsData _BaseCharacterInputData;
}
