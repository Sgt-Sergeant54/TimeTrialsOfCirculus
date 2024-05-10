using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStanding : IPlayerState
{
    public void Enter(PlayerController player)
    {
        return;
    }

    public void Exit(PlayerController player)
    {
        return;
    }

    public IPlayerState Tick(PlayerController player, PlayerInputs input)
    {
        if (input.onWall && player.groundCheck() == false) return new PlayerOnWall();
        if (input.grapple) return new PlayerGrapple();
        if (player.groundCheck() && input.jump) return new PlayerJumping();
        if (!player.groundCheck() && input.jump && player.jumpTwice)
        {
            return new PlayerJumping();
        }

        player.direction.x = input.moveHorizontal;

        return null;
    }
}
