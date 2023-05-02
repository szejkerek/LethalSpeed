public abstract class StateEnemyAI
{
    protected StateFactoryEnemyAI _factory;
    protected StateMachineEnemyAI _context;
    protected string stateName;

    public StateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName)
    {
        _factory = factory;
        _context = context;
        this.stateName = stateName;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract DebugEnemyAIText GetDebugText();
    public void SwitchState(StateEnemyAI newState) 
    {
        _context.CurrentState.ExitState();
        newState.EnterState();
        _context.CurrentState = newState;
    }
}
