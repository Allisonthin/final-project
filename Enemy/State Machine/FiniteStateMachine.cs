using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine 
{
    public State currentstate { get; private set;}
    public void Initialize(State startingState)
    {
        currentstate = startingState;
        currentstate.Enter();
    }

    public void ChangeState(State newState)
    {
        currentstate.Exit();
        currentstate = newState;
        currentstate.Enter();
    }
}
