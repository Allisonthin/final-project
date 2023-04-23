using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this call is where we create variable that hold reference to our "current state", "function to initialize current state", "function to change current state".  
public class PlayerStateMachine 
{
    public PlayerState CurrentState { get; private set; }


    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
