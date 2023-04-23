using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int AmountOfJumpLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        AmountOfJumpLeft = playerData.amountOfJump;
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();

        core.Movement.SetVelocityY(playerData.jumpVelocity);

        isAbilityDone = true;
        AmountOfJumpLeft--;
        player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {
        if (AmountOfJumpLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpLeft() => AmountOfJumpLeft = playerData.amountOfJump;

    public void DecreaseAmountOfJumpLeft() => AmountOfJumpLeft--;
}
