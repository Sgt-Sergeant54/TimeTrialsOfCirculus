using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HazardTypes
{
    Laser,
    Spikes
}

public class Hazard : MonoBehaviour
{
    public float damageTimer;
    public float damageEffect;
    public HazardTypes type;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>() != null)
        {
            if (col.GetComponent<Damage>() == null)
            {
                Damage hazard = col.gameObject.AddComponent<Damage>();
                hazard.damageEffect = damageEffect;
                hazard.damageTimer = damageTimer;
            }
            else
            {
                col.gameObject.GetComponent<PlayerController>().Respawn();
            }
        }
    }
}
