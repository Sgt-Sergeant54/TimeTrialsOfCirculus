using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost : Effect
{

    public override void RemoveEffect()
    {
        if (GetComponent<GravityInverter>() != null)
        {
            GetComponent<PlayerController>().jumpHeight = -20f;
        }
        else
        {
            GetComponent<PlayerController>().jumpHeight = 20f;
        }
    }

    public override void StartEffect()
    {
        GetComponent<PlayerController>().jumpHeight = 20f * effectStrength;
    }
}
