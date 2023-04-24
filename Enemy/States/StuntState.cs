using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntState : State
{
    protected D_StuntState stateData;

    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    


    public StuntState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StuntState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Docheck()
    {
        base.Docheck();

        isGrounded = core.CollisionSenses.Ground;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
        core.Movement.SetVelocity(stateData .stunKnockBackSpeed , stateData .StuntKnockBackAngle ,entity .lastDamageDirection );
    }

    public override void Exit()
    {
        base.Exit();

        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startingTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }

        if(isGrounded && Time.time >= startingTime  + stateData .stunKnockBackTime  && !isMovementStopped )
        {
            isMovementStopped = true;
            core.Movement.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
