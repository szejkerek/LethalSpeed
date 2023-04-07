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
    public void UpdateState()
    {
        CheckSwitchState();
        UpdateStateInternally();
    }
    public abstract void UpdateStateInternally();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public void SwitchState(EnemyAIState newState) 
    {
        _context.CurrentState.ExitState();
        newState.EnterState();
        _context.CurrentState = newState;
    }
}
