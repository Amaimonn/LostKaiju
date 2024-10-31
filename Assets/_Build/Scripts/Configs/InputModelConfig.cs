using UnityEngine;

public abstract class InputModelConfig : ScriptableObject
{
    public abstract CharacterModel GetModel();
}