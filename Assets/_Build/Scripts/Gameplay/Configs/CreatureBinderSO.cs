using UnityEngine;

using LostKaiju.Gameplay.Creatures.Views;

namespace LostKaiju.Gameplay.Configs 
{
    [CreateAssetMenu(fileName = "CreatureBinderSO", menuName = "Scriptable Objects/Creature Binder SO")]
    public class CreatureBinderSO : ScriptableObject, ICreatureConfig
    {
        [field: SerializeField] public CreatureBinder CreatureBinder { get; private set;}
    }
}
