using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInverter : Effect
{
    public override void RemoveEffect()
    {
        if (GetComponent<JumpBoost>() != null)
        {
            GetComponent<PlayerController>().jumpHeight = 20 * GetComponent<JumpBoost>().effectStrength;
        }
        else
        {
            GetComponent<PlayerController>().jumpHeight = 20f;
        }
        GetComponent<Rigidbody2D>().gravityScale *= -1;
    }

    public override void StartEffect()
    {
        GetComponent<Rigidbody2D>().gravityScale = -6f;
        GetComponent<PlayerController>().jumpHeight *= -1;
    }
}
