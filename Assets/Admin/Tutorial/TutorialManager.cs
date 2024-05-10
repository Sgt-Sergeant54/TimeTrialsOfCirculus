using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Vector3 KingsLocation;
    private bool hasBeenStarted;
    private bool movingToward;
    private bool talkingToKing;

    [SerializeField] private CanvasGroup[] conversationWithKing;
    private int index;

    PlayerController player;

    private void Start()
    {
        KingsLocation = new Vector3(-39f, -3.49f, 0);
        movingToward = false;
        talkingToKing = false;
        hasBeenStarted = false;
        index = 0;
    }

    private void Update()
    {
        if (talkingToKing)
        {
            if (conversationWithKing[index].alpha != 1)
            {
                conversationWithKing[index].alpha = 1;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                conversationWithKing[index].alpha = 0;
                index++;
                if (index == conversationWithKing.Length)
                {
                    talkingToKing = false;
                    player.enabled = true;
                    conversationWithKing[0].alpha = 0;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (movingToward)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, KingsLocation, Time.fixedDeltaTime * player.playerSpeed);
            if (player.transform.position == KingsLocation)
            {
                movingToward = false;
                talkingToKing = true;
                conversationWithKing[index].alpha = 1;
                index++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasBeenStarted)
        {
            movingToward = true;
            player = col.GetComponent<PlayerController>();
            player.lineRenderer.enabled = false;
            player.distanceJoint.enabled = false;
            player.enabled = false;
            hasBeenStarted = true;
        }
    }
}
