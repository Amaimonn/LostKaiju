using UnityEngine;

using LostKaiju.Creatures.Presenters;

namespace LostKaiju.Configs 
{
    public abstract class CreaturePresenterSO : ScriptableObject
    {
        public abstract CreaturePresenter GetPresenter();
    }
}
