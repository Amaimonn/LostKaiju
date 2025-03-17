using R3;

namespace LostKaiju.Infrastructure.Loading
{
    public interface ILoadingScreenNotifier
    {
        public Observable<Unit> OnStarted { get; }
        public Observable<Unit> OnFinished { get; }
    }
}