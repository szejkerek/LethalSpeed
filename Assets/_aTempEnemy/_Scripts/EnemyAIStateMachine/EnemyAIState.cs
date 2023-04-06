using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class EnemyAIState
{
    protected EnemyAIStateFactory _factory;
    protected EnemyAIStateMachine _context;
    public EnemyAIState(EnemyAIStateMachine context, EnemyAIStateFactory factory)
    {
        _factory = factory;
        _context = context;
    }

    public abstract void EnterState();
    public abstract void UpdateStateInternally();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public void UpdateState()
    {
        CheckSwitchState();
        UpdateState();
    }
    protected void SwitchState(EnemyAIState newState) 
    {
        ExitState();
        newState.EnterState();
        _context.CurrentState = newState;
    }
}
