using ObservableCollections;
using R3;

namespace LostKaiju.Game.World.Missions.Triggers
{
    public class GroupTriggerObserver<T>
    {
        public IObservableCollection<T> CurrentMembers => _currentMembers;
        private readonly ObservableHashSet<T> _currentMembers = new();

        public GroupTriggerObserver(TargetTrigger<T> targetTrigger)
        {
            targetTrigger.OnEnter.Subscribe(OnMemberEnter);
            targetTrigger.OnExit.Subscribe(OnMemberExit);
        }
        
        protected virtual void OnMemberEnter(T target)
        {
            _currentMembers.Add(target);
        }

        protected virtual void OnMemberExit(T target)
        {
            _currentMembers.Remove(target);
        }
    }
}