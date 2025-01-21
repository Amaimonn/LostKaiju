using UnityEngine;

namespace LostKaiju.Game.Creatures.Presenters 
{
    public abstract class CreaturePresenterSO : ScriptableObject
    {
        public abstract CreaturePresenter GetPresenter();
    }
}
