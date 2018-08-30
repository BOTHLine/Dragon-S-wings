using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerPart : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float lifeTime;

    public void Init(SpriteRenderer originalRenderer, float lifeTime)
    {
        spriteRenderer.sprite = originalRenderer.sprite;
        transform.position = originalRenderer.transform.position;
        this.lifeTime = lifeTime;

        spriteRenderer.color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        transform.localScale = Vector3.one;
    }

    public bool CustomUpdate(float time)
    {
        lifeTime -= time;

        if (lifeTime < 0.0f)
            return true;
        return false;
    }
}