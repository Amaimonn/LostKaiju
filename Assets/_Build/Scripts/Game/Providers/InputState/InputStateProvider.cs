using ObservableCollections;
using R3;

namespace LostKaiju.Game.Providers.InputState
{
    public class InputStateProvider
    {
        public Observable<bool> IsInputEnabled => _isInputEnabled;
        private readonly ReactiveProperty<bool> _isInputEnabled;
        private readonly ObservableHashSet<IInputBlocker> _inputBlockers = new();

        public InputStateProvider()
        {
            _isInputEnabled = new ReactiveProperty<bool>(true);
            _inputBlockers.ObserveCountChanged().Subscribe(x => _isInputEnabled.Value = x == 0);
        }

        public void AddBlocker(IInputBlocker inputBlocker)
        {
            _inputBlockers.Add(inputBlocker);
        }

        public void RemoveBlocker(IInputBlocker inputBlocker)
        {
            _inputBlockers.Remove(inputBlocker);
        }
    }
}
