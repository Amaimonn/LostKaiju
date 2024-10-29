using UnityEngine;

public abstract class InputModelConfig : ScriptableObject
{
    public abstract InputModel GetModel();
}