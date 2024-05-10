using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplePull : IPlayerState
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
        //if player input is no longer space then return to grapple
        if (input.grappleIn == false) return new PlayerGrapple();

        player.distanceJoint.distance -= player.grappleSpeed * Time.deltaTime;
        player.lineRenderer.SetPosition(1, player.transform.position);

        player.direction.x = input.moveHorizontal;
        return null;
    }
}
