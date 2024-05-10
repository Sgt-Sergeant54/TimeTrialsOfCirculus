using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    public float effectDuration;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();
        rb2d.gravityScale = 0f;
    }

    void Update()
    {
        effectDuration -= Time.deltaTime;
        if (effectDuration < 0)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        rb2d.gravityScale = 6f;
    }
}
