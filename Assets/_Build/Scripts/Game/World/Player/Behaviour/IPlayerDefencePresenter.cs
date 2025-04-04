using System;
using LostKaiju.Game.World.Creatures.Presenters;

namespace LostKaiju.Game.World.Player.Behaviour
{
    public interface IPlayerDefencePresenter : ICreaturePresenter, IDisposable
    {
        public void SetInvincible(bool isInvincible);
    }
}