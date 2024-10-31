using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelSO", menuName = "Scriptable Objects/PlayerModelSO")]
public class PlayerModelSO : InputModelConfig
{
    public override CharacterModel GetModel() => new PlayerModel(_BaseCharacterInputData);

    [SerializeField] private PlayerControlsData _BaseCharacterInputData;
}
