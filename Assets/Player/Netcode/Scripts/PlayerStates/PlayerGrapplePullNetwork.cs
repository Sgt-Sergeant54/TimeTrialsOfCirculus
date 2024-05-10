using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplePullNetwork : IPlayerStateNetwork
{
    private PlayerInputs inputs;

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
        //if player input is no longer space then return to grapple
        if (input.grappleIn == false) return new PlayerGrappleNetwork();

        player.distanceJoint.distance -= player.grappleSpeed * Time.deltaTime;
        player.lineRenderer.SetPosition(1, player.transform.position);

        player.direction.Value = input.moveHorizontal;
        
        return null;
    }
}
