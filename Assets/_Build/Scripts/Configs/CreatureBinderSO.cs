using UnityEngine;

using LostKaiju.Creatures.Views;

namespace LostKaiju.Configs 
{
    [CreateAssetMenu(fileName = "CreatureBinderSO", menuName = "Scriptable Objects/Creature Binder SO")]
    public class CreatureBinderSO : ScriptableObject, ICreatureConfig
    {
        [field: SerializeField] public CreatureBinder CreatureBinder { get; private set;}
    }
}
