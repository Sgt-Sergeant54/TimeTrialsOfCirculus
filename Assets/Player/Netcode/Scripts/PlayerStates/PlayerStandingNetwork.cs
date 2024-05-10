using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingNetwork : IPlayerStateNetwork
{
    public void Enter(PlayerControllerNetwork player)
    {
        return;
    }

    public void Exit(PlayerControllerNetwork player)
    {
        return;
    }

    public IPlayerStateNetwork Tick(PlayerControllerNetwork player, PlayerInputs input)
    {
        if (input.onWall && player.groundCheck() == false) return new PlayerOnWallNetwork();
        if (input.grapple) return new PlayerGrappleNetwork();
        if (player.groundCheck() && input.jump) return new PlayerJumpingNetwork();
        if (!player.groundCheck() && input.jump && player.jumpTwice)
        {
            return new PlayerJumpingNetwork();
        }

        player.direction.Value = input.moveHorizontal;

        return null;
    }
}
