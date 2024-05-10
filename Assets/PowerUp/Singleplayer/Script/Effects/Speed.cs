using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : Effect
{

    public override void RemoveEffect()
    {
        if (GetComponent<Slow>() != null)
        {
            GetComponent<PlayerController>().playerSpeed = 13 * GetComponent<Slow>().effectStrength;
        }
        else
        {
            GetComponent<PlayerController>().playerSpeed = 13f;
        }
    }

    public override void StartEffect()
    {
        GetComponent<PlayerController>().playerSpeed *= effectStrength;
    }
}
