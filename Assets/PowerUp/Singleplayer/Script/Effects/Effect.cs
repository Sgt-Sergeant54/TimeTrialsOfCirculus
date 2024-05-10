using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public float effectTimer;
    public float effectStrength;

    public abstract void StartEffect();
    public abstract void RemoveEffect();

    private void Awake()
    {
        playSoundEffect();
    }

    protected void Update()
    {
        effectTimer -= Time.deltaTime;
        if (effectTimer <= 0)
        {
            Destroy(this);
        }
    }

    protected void OnDestroy()
    {
        RemoveEffect();
    }

    protected void playSoundEffect()
    {
        levelAudioManager.Instance.powerUp();
    }
}
