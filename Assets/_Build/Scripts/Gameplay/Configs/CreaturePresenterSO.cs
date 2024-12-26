using UnityEngine;

using LostKaiju.Gameplay.Creatures.Presenters;

namespace LostKaiju.Gameplay.Configs 
{
    public abstract class CreaturePresenterSO : ScriptableObject
    {
        public abstract CreaturePresenter GetPresenter();
    }
}
