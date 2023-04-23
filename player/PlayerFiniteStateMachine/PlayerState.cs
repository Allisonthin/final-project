using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Core core;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    //tell which animation should play
    private string animBoolName;



    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;

        core = player.Core;
    }

    // enter is call when we enter specific state
    public virtual void Enter()
    {
        DoCheck();
        player.Anim.SetBool(animBoolName, true);

        startTime = Time.time;

        //Debug.Log(animBoolName);

        isAnimationFinished = false;
        isExitingState = false;
    }

    // exit is call when we exit a state
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    // logicUpdate is call every frame
    public virtual void LogicUpdate()
    {

    }

    // physicsUpdate is call every fixedUpdate
    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }

    // doCheck is to check thing like check for ground, wall etc . It is call from enter and physicupdate function
    public virtual void DoCheck()
    {

    }

    public virtual void AnimationTrigger(){}

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
