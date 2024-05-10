using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Effect
{
    public override void RemoveEffect()
    {
        if (GetComponent<Speed>() != null)
        {
            GetComponent<PlayerController>().playerSpeed = 13 * GetComponent<Speed>().effectStrength;
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
