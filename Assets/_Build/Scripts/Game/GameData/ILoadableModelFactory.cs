using System;
using R3;

namespace LostKaiju.Game.GameData
{
    public interface ILoadableModelFactory<T> where T : IModel
    {
        public Observable<T> OnProduced { get; }

        public abstract void GetModelOnLoaded(Action<T> onLoaded);
    }
}