using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{

    protected D_MoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;

    protected bool isplayerInMinAgroRange;
    //protected bool isplayerInMaxAgroRange;
    public MoveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(etity, stateMachine, animBoolName)
    {

        this.stateData = stateData;
    }
    public override void Docheck()
    {
        base.Docheck();

        isDetectingLedge = entity.checkLedge();
        isDetectingWall = entity.checkWall();

        isplayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);

        
        //isplayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
        //isplayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();


    }
}
