using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIStateMachine : MonoBehaviour
{
    EnemyAIState _currentState;
    EnemyAIStateFactory _statesFactory;

    public EnemyAIState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public EnemyAIStateFactory StatesFactory => _statesFactory;

    private void Awake()
    {
        _statesFactory = new EnemyAIStateFactory(this);
        _currentState = _statesFactory.Idle();
        _currentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }
}
