using UnityEngine;

using LostKaiju.Game.Creatures.Presenters;

namespace LostKaiju.Game.Configs 
{
    public abstract class CreaturePresenterSO : ScriptableObject
    {
        public abstract CreaturePresenter GetPresenter();
    }
}
