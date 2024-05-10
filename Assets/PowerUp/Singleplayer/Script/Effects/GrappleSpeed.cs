using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleSpeed : Effect
{
    private float originalValue;

    public override void RemoveEffect()
    {
        GetComponent<PlayerController>().grappleSpeed = originalValue;
    }

    public override void StartEffect()
    {
        originalValue = GetComponent<PlayerController>().grappleSpeed;
        GetComponent<PlayerController>().grappleSpeed = originalValue * effectStrength;
    }
}
