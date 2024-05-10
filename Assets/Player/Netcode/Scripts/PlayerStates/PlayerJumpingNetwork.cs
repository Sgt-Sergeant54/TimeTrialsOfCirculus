using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerJumpingNetwork : IPlayerStateNetwork
{
    public void Enter(PlayerControllerNetwork player)
    {
        Debug.Log("jump");
        player.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.jumpHeight);
        return;
    }

    public void Exit(PlayerControllerNetwork player)
    {
        Debug.Log("leave jump");
        return;
    }

    public IPlayerStateNetwork Tick(PlayerControllerNetwork player, PlayerInputs input)
    {
        if (input.grapple) return new PlayerGrappleNetwork();
        if (input.onWall) return new PlayerOnWallNetwork();
        if (player.groundCheck()) return new PlayerStandingNetwork();
        if (!player.groundCheck() && input.jump && player.jumpTwice)
        {
            return new PlayerJumpingNetwork();
        }

        player.direction.Value = input.moveHorizontal;

        return null;
    }
}
