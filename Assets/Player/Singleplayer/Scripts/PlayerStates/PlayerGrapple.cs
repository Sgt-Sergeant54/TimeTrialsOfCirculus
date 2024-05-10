using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerGrapple : IPlayerState
{
    private PlayerInputs inputs;

    public void Enter(PlayerController player)
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
            }
        }

        return;
    }

    public void Exit(PlayerController player)
    {
        if (inputs.grapple == false)
        {
            player.distanceJoint.enabled = false;
            player.lineRenderer.enabled = false;
        }
        return;
    }

    public IPlayerState Tick(PlayerController player, PlayerInputs input)
    {
        inputs = input;
        if (input.grapple == false) return new PlayerStanding();

        if (input.grappleIn) return new PlayerGrapplePull();

        player.direction.x = input.moveHorizontal;
        player.lineRenderer.SetPosition(1, player.transform.position);
        return null;
    }
}
