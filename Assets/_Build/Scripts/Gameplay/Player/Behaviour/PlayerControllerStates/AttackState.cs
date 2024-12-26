using R3;

using LostKaiju.Gameplay.Creatures.Features;

namespace LostKaiju.Gameplay.Player.Behaviour.PlayerControllerStates
{
    public class AttackState : PlayerControllerState
    {
        public ReadOnlyReactiveProperty<bool> IsCompleted => _isCompleted;

        protected float _currentDuration;
        protected ReactiveProperty<bool> _isCompleted = new(false);
        protected IAttacker _attacker;

        public void Init(IAttacker attacker)
        {
            _attacker = attacker;
        }

        public override void UpdateLogic()
        {
            HandleTransitions();
        }

        public override void Enter()
        {
            base.Enter();
            _isCompleted.Value = false;
            _currentDuration = 0;
            _attacker.Attack();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void Attack()
        {

        }

    }
}