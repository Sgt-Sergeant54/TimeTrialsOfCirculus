using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : IPlayerState
{
    public void Enter(PlayerController player)
    {
        Debug.Log("jump");
        player.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.jumpHeight);
        return;
    }

    public void Exit(PlayerController player)
    {
        Debug.Log("leave jump");
        return;
    }

    public IPlayerState Tick(PlayerController player, PlayerInputs input)
    {
        if (input.grapple) return new PlayerGrapple();
        if (input.onWall) return new PlayerOnWall();
        if (player.groundCheck()) return new PlayerStanding();
        if (!player.groundCheck() && input.jump && player.jumpTwice)
        {
            return new PlayerJumping();
        }

        player.direction.x = input.moveHorizontal;
        return null;
    }
}
