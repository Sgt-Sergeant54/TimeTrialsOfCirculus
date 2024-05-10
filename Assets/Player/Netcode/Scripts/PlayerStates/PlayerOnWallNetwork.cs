using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnWallNetwork : IPlayerStateNetwork
{
    public void Enter(PlayerControllerNetwork player)
    {
        //set the gravity scale to 0.2 ish
        if (player.GetComponent<GravityInverterNetwork>() == null)
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().gravityScale = -0.2f;
        }

        player.GetComponent<Rigidbody2D>().velocityY = 0f;
        player.direction.Value = 0;
        Debug.Log("Wall");
        return;
    }

    public void Exit(PlayerControllerNetwork player)
    {
        if (!player.GetComponent<GravityInverterNetwork>())
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 6f;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().gravityScale = -6f;
        }

        player.isOnWall = false;
        return;
    }

    public IPlayerStateNetwork Tick(PlayerControllerNetwork player, PlayerInputs input)
    {
        if (input.grapple) return new PlayerGrappleNetwork();
        if (input.onWall == false) return new PlayerStandingNetwork();

        if (player.groundCheck()) return new PlayerStandingNetwork();

        if (input.jump) return new PlayerJumpingNetwork();

        if (player.GetComponent<GravityInverterNetwork>() == null)
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        }

        return null;
    }
}
