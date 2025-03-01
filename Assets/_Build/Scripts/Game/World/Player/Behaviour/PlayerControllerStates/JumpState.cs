using UnityEngine;

using LostKaiju.Game.World.Player.Data.StateParameters;

namespace LostKaiju.Game.World.Player.Behaviour.PlayerControllerStates
{
    public class JumpState : PlayerControllerState
    {
        protected JumpParameters _parameters;
        protected Rigidbody2D _rigidbody;
        
        public void Init(JumpParameters parameters, Rigidbody2D rigidbody)
        {
            _parameters = parameters;
            _rigidbody = rigidbody;
        }

        public override void UpdateLogic()
        {
            HandleTransitions();
        }

        public override void Enter()
        {
            base.Enter();
            Jump();
        }

        private void Jump()
        {
            _rigidbody.linearVelocityY = 0;
            _rigidbody.AddForceY(_parameters.Force, ForceMode2D.Impulse);
        }    
    }
}
