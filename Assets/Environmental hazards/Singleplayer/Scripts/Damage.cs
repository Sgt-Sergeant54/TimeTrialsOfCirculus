using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damageTimer;
    public float damageEffect;

    private void Update()
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer < 0)
        {
            Destroy(this);
        }
    }
}
