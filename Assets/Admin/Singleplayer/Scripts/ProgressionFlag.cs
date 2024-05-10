using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionFlag : MonoBehaviour
{
    public bool IsPassed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            IsPassed = true;
        }
    }
}
