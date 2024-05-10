using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Effect
{
    public override void RemoveEffect()
    {
        GetComponent<PlayerController>().jumpTwice = false;
    }

    public override void StartEffect()
    {
        GetComponent<PlayerController>().jumpTwice = true;
    }
}
