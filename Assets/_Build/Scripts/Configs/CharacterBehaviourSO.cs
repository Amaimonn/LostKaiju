using UnityEngine;

using LostKaiju.Player.Behaviour;

namespace LostKaiju.Configs 
{
    public abstract class CharacterBehaviourSO : ScriptableObject
    {
        public abstract CharacterBehaviour GetModel();
    }
}
