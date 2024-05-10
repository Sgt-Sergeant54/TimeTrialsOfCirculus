using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum debuffType
{
    Slow
}

public class Debuffer : MonoBehaviour
{
    [SerializeField] private debuffType type;
    [SerializeField] private float effectStrength;
    [SerializeField] private float effectLength;

    private void applyEffect(GameObject player)
    {
        Effect effect;

        switch (type)
        {
            case debuffType.Slow:
                if (player.GetComponent<Slow>() == null)
                {
                    effect = player.AddComponent<Slow>();
                }
                else
                {
                    effect = player.GetComponent<Slow>();
                }
                effect.effectStrength = effectStrength;
                effect.effectTimer = effectLength;
                effect.StartEffect();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>() != null)
        {
            applyEffect(col.gameObject);
            Destroy(gameObject);
        }
    }

}
