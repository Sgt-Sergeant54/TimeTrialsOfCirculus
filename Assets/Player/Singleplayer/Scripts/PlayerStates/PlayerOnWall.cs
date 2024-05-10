using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnWall : IPlayerState
{
    public void Enter(PlayerController player)
    {
        //set the gravity scale to 0.2 ish
        if (player.GetComponent<GravityInverter>() == null)
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().gravityScale = -0.2f;
        }

        player.GetComponent<Rigidbody2D>().velocityY = 0f;
        player.direction = Vector3.zero;
        Debug.Log("Wall");
        return;
    }

    public void Exit(PlayerController player)
    {
        if (!player.GetComponent<GravityInverter>())
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 6f;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().gravityScale = -6f;
        }
        
        player.IsOnWall = false;
        return;
    }

    public IPlayerState Tick(PlayerController player, PlayerInputs input)
    {
        if (input.grapple) return new PlayerGrapple();
        if (input.onWall == false) return new PlayerStanding();

        if (player.groundCheck()) return new PlayerStanding();

        if (input.jump) return new PlayerJumping();

        if (player.GetComponent<GravityInverter>() == null)
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        }

        return null;
    }
}
