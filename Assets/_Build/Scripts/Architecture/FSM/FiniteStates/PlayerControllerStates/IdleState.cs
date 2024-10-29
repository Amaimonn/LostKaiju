public class IdleState: FiniteState
{
    public override void UpdateLogic()
    {
        HandleTransitions();
    }
}