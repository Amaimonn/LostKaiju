using UnityEngine;

namespace LostKaiju.Game.World.Creatures.Presenters 
{
    public abstract class CreaturePresenterSO : ScriptableObject
    {
        public abstract ICreaturePresenter GetPresenter();
    }
}
