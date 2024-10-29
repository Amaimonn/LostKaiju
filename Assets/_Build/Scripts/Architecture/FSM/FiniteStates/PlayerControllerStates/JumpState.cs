using UnityEngine;

public class JumpState : PlayerControllerState
{
    protected JumpParameters _parameters;
    
    public void Init(JumpParameters parameters)
    {
        _parameters = parameters;
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
        _parameters.JumpRigidbody.linearVelocityY = 0;
        _parameters.JumpRigidbody.AddForceY(_parameters.JumpForce, ForceMode2D.Impulse);
    }    
}