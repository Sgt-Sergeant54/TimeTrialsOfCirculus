using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointFlag : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (LevelManager.instance.endFlag  == this)
            {
                LevelManager.instance.newLap();
            }
            else
            {
                LevelManager.instance.currentCheckPoint = transform.position;
            }
        }
    }
}
