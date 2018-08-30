using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerPart : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float lifeTime;
    public float alphaDecreasePerSecond;

    public void Init(SpriteRenderer originalRenderer, float lifeTime)
    {
        spriteRenderer.sprite = originalRenderer.sprite;
        transform.position = originalRenderer.transform.position;
        this.lifeTime = lifeTime;
        
        spriteRenderer.color = Color.white;
        transform.localScale = Vector3.one;

        alphaDecreasePerSecond = spriteRenderer.color.a / lifeTime;

    }

    public bool CustomUpdate(float time)
    {
        lifeTime -= time;

        Color color = spriteRenderer.color;
        color.a -= alphaDecreasePerSecond * time;
        spriteRenderer.color = color;

        if (lifeTime < 0.0f)
            return true;
        return false;
    }
}