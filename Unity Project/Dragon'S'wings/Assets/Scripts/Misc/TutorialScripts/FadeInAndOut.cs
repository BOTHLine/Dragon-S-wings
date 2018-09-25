using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOut : MonoBehaviour
{
    public float minimum = 0.0f;
    public float maximum = 1f;
    public float speed = 5.0f;
    public float threshold = float.Epsilon;

    public bool faded = false;
    public bool fadeIsActive = false;

    public SpriteRenderer sprite;

    void Start()
    {
        sprite.color = new Color(1f, 1f, 1f, 0);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (!fadeIsActive) activateFading();
            else
            {
                faded = !faded;
            }
        }
    }

    void OnTriggerExit2D()
    {
        faded = !faded;
    }


    void Update()
    {
        if (fadeIsActive)
        {
            float step = speed * Time.deltaTime;

            if (faded)
            {
                sprite.color = new Color(1f, 1f, 1f, Mathf.Lerp(sprite.color.a, maximum, step));
                if (Mathf.Abs(maximum - sprite.color.a) <= threshold)
                    faded = false;

            }
            else
            {
                sprite.color = new Color(1f, 1f, 1f, Mathf.Lerp(sprite.color.a, minimum, step));
                if (Mathf.Abs(sprite.color.a - minimum) <= threshold)
                    faded = true;
            }
        }
    }

    public void activateFading()
    {
        fadeIsActive = true;
    }
}
