using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class StateEnemyAI
{
    protected StateFactoryEnemyAI _factory;
    protected StateMachineEnemyAI _context;
    public StateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory)
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
    public abstract DebugEnemyAIText GetDebugText();
    public void SwitchState(StateEnemyAI newState) 
    {
        _context.CurrentState.ExitState();
        newState.EnterState();
        _context.CurrentState = newState;
    }
}