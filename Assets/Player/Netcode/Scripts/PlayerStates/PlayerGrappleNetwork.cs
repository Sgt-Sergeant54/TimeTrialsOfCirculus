using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleNetwork : IPlayerStateNetwork
{
    private PlayerInputs inputs;

    public void Enter(PlayerControllerNetwork player)
    {
        if (player.distanceJoint.enabled == false)
        {
            Vector2 mousePos = player.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = (Vector2)player.transform.position;

            UnityEngine.Vector2 direction = mousePos - playerPosition;
            LayerMask mask = LayerMask.GetMask("Ground");

            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 100, mask);

            if (hit.collider != null)
            {
                player.lineRenderer.SetPosition(0, hit.point);
                player.lineRenderer.SetPosition(1, player.transform.position);
                player.distanceJoint.connectedAnchor = hit.point;
                player.distanceJoint.distance = UnityEngine.Vector3.Distance(hit.point, playerPosition);
                player.distanceJoint.enabled = true;
                player.lineRenderer.enabled = true;
                player.grappleActive.Value = true;
                player.grappleX.Value = hit.point.x;
                player.grappleY.Value = hit.point.y;
            }
        }
        return;
    }

    public void Exit(PlayerControllerNetwork player)
    {
        if (inputs.grapple == false)
        {
            player.distanceJoint.enabled = false;
            player.lineRenderer.enabled = false;
            player.grappleActive.Value =false;
        }
        return;
    }

    public IPlayerStateNetwork Tick(PlayerControllerNetwork player, PlayerInputs input)
    {
        inputs = input;
        if (input.grapple == false) return new PlayerStandingNetwork();

        if (input.grappleIn) return new PlayerGrapplePullNetwork();

        player.direction.Value = input.moveHorizontal;

        player.lineRenderer.SetPosition(1, player.transform.position);
        return null;
    }
}
