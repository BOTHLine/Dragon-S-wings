using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    public float blinkDuration;
    public float blinkTimeReduction;
    public float showTime;

    public bool waitTimeDone = false;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void StartBlinking()
    {
        ResetBlinker();
        spriteRenderer.enabled = true;
        StartCoroutine(Blinking(blinkDuration));
    }

    public void ResetBlinker()
    {
        spriteRenderer.enabled = false;
    }

    IEnumerator Wait(float waitTime)
    {     
        yield return new WaitForSeconds(waitTime);        
    }

    IEnumerator Blinking(float duration)
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
        duration -= blinkTimeReduction;
        yield return new WaitForSeconds(duration);
        if (duration > 0)
        {
            StartCoroutine(Blinking(duration));
        }
        else
        {
            StartCoroutine(ShowMeFor(showTime));
        }
    }

    IEnumerator ShowMeFor(float time)
    {
        spriteRenderer.enabled = true;
        waitTimeDone = true;
        yield return new WaitForSeconds(time);
        spriteRenderer.enabled = false;
        waitTimeDone = false;
    }
}