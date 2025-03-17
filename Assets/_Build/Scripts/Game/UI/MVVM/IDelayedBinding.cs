using R3;

namespace LostKaiju.Game.UI.MVVM
{
    public interface IDelayedBinding
    {
        public Observable<bool> IsLoaded { get; }
    }
}