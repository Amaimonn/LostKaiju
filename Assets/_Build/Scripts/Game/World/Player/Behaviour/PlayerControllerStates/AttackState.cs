using R3;

using LostKaiju.Game.World.Creatures.Features;

namespace LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates
{
    public class AttackState : PlayerControllerState
    {
        public ReadOnlyReactiveProperty<bool> IsAttackReady => _isAttackReady;
        public ReadOnlyReactiveProperty<bool> IsAttackCompleted => _isAttackCompleted;

        protected float _currentDuration;
        protected ReactiveProperty<bool> _isAttackReady = new(true);
        protected ReactiveProperty<bool> _isAttackCompleted = new(true);
        protected IAttacker _attacker;

        public AttackState(IAttacker attacker)
        {
            _attacker = attacker;
            _attacker.OnAttackCompleted.Subscribe(_ => _isAttackReady.Value = true);
            _attacker.OnAttackCompleted.Subscribe(_ => _isAttackCompleted.Value = true);
        }

        public override void UpdateLogic()
        {
            HandleTransitions();
        }

        public override void Enter()
        {
            base.Enter();
            Attack();
        }

        private void Attack()
        {
            _isAttackReady.Value = false;
            _isAttackCompleted.Value = false;
            _currentDuration = 0;
            _attacker.Attack();
        }

    }
}