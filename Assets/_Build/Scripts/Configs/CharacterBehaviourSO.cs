using UnityEngine;

using Assets._Build.Scripts.Player.Behaviour;

namespace Assets._Build.Scripts.Configs
{
    public abstract class CharacterBehaviourSO : ScriptableObject
    {
        public abstract CharacterBehaviour GetModel();
    }
}
