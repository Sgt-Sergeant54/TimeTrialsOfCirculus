using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInverterNetwork : Effect
{
    public override void RemoveEffect()
    {
        if (GetComponent<JumpBoost>() != null)
        {
            GetComponent<PlayerControllerNetwork>().jumpHeight = 20 * GetComponent<JumpBoost>().effectStrength;
        }
        else
        {
            GetComponent<PlayerControllerNetwork>().jumpHeight = 20f;
        }
        GetComponent<Rigidbody2D>().gravityScale *= -1;
    }

    public override void StartEffect()
    {
        GetComponent<Rigidbody2D>().gravityScale = -6f;
        GetComponent<PlayerControllerNetwork>().jumpHeight *= -1;
    }
}
