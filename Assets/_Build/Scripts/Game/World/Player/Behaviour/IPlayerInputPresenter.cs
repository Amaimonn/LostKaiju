using System;
using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public interface IPlayerInputPresenter : ICreaturePresenter, IUpdatablePresenter, IDisposable
    {
        public void SetInputEnabled(bool isEnabled);
    }
}