using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Speed,
    Jump,
    ExtraJump,
    TeleDistance,
    GrappleSpeed,
    GravityInverter
}

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType type;
    [SerializeField] private float effectStrength;
    [SerializeField] private float effectLength;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem particles;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(type == PowerUpType.Speed)
        {
            spriteRenderer.color = Color.blue;
            ParticleSystem.MainModule psMain = particles.main;
            psMain.startColor = Color.blue;
        }
        else if (type == PowerUpType.Jump)
        {
            spriteRenderer.color = Color.red;
            ParticleSystem.MainModule psMain = particles.main;
            psMain.startColor = Color.red;
        }else if (type == PowerUpType.ExtraJump)
        {
            spriteRenderer.color = Color.white;
            ParticleSystem.MainModule psMain = particles.main;
            psMain.startColor = Color.white;
        }else if (type == PowerUpType.TeleDistance)
        {
            spriteRenderer.color = Color.magenta;
            ParticleSystem.MainModule psMain = particles.main;
            psMain.startColor = Color.magenta;
        }else if (type == PowerUpType.GrappleSpeed)
        {
            spriteRenderer.color = Color.yellow;
            ParticleSystem.MainModule psMain = particles.main;
            psMain.startColor = Color.yellow;
        }else if (type == PowerUpType.GravityInverter)
        {
            spriteRenderer.color = Color.black;
            ParticleSystem.MainModule psMain = particles.main;
            psMain.startColor = Color.black;
        }
    }

    private void applyEffect(GameObject player)
    {
        Effect effect;

        switch (type)
        {
            case PowerUpType.Jump:
                if (player.GetComponent<JumpBoost>() == null)
                {
                    effect = player.AddComponent<JumpBoost>();
                }
                else
                {
                    effect = player.GetComponent<JumpBoost>();
                }
                effect.effectStrength = effectStrength;
                effect.effectTimer = effectLength;
                effect.StartEffect();
                break;

            case PowerUpType.ExtraJump:
                if (player.GetComponent<DoubleJump>() == null)
                {
                    effect = player.AddComponent<DoubleJump>();
                }
                else
                {
                    effect = player.GetComponent<DoubleJump>();
                }
                effect.effectStrength = effectStrength;
                effect.effectTimer = effectLength;
                effect.StartEffect();
                break;

            case PowerUpType.TeleDistance:
                if (player.GetComponent<TeleDistance>() == null)
                {
                    effect = player.AddComponent<TeleDistance>();
                }
                else
                {
                    effect = player.GetComponent<TeleDistance>();
                }
                effect.effectStrength = effectStrength;
                effect.effectTimer = effectLength;
                effect.StartEffect();
                break;

            case PowerUpType.GrappleSpeed:
                if (player.GetComponent<GrappleSpeed>() == null)
                {
                    effect = player.AddComponent<GrappleSpeed>();
                }
                else
                {
                    effect = player.GetComponent<GrappleSpeed>();
                }
                effect.effectStrength = effectStrength;
                effect.effectTimer = effectLength;
                effect.StartEffect();
                break;

            case PowerUpType.Speed:
                if (player.GetComponent<Speed>() == null)
                {
                    effect = player.AddComponent<Speed>();
                }
                else
                {
                    effect= player.GetComponent<Speed>();
                }
                effect.effectStrength = effectStrength;
                effect.effectTimer = effectLength;
                effect.StartEffect();
                break;

            case PowerUpType.GravityInverter:
                if (player.GetComponent<GravityInverter>() == null)
                {
                    effect = player.AddComponent<GravityInverter>();
                }
                else
                {
                    effect = player.GetComponent<GravityInverter>();
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
