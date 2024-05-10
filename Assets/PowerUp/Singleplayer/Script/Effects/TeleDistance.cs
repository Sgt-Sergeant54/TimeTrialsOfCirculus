using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleDistance : Effect
{
    private float originalValue;

    public override void RemoveEffect()
    {
        GetComponent<PlayerController>().teleDistance = originalValue;
    }

    public override void StartEffect()
    {
        originalValue = GetComponent<PlayerController>().teleDistance;
        GetComponent<PlayerController>().teleDistance = originalValue * effectStrength;
    }
}
