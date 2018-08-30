using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerInformation : MonoBehaviour
{
    public SpriteRenderer originalRenderer;
    public float timeLastPart = 0.0f;
    public float partDistance;
    public float lifeTime;
    public float alphaDecreasePerSecond;
    public float scaleDecreasePerSecond;
    public List<TrailerPart> trailerParts = new List<TrailerPart>();

    public void Init(SpriteRenderer originalRenderer, float partDistance, float lifeTime, float targetLastScale)
    {
        this.originalRenderer = originalRenderer;
        this.partDistance = partDistance;
        timeLastPart = partDistance;
        this.lifeTime = lifeTime;

        alphaDecreasePerSecond = 255.0f / lifeTime;
        scaleDecreasePerSecond = (1.0f - targetLastScale) / lifeTime;
    }

    public void CustomUpdate(float time)
    {
        for (int i = 0; i < trailerParts.Count; i++)
        {
            TrailerPart trailerPart = trailerParts[i];
            Debug.Log("Color before: " + trailerPart.spriteRenderer.color);
            float newValue = trailerPart.spriteRenderer.color.a - alphaDecreasePerSecond * time;
            trailerPart.spriteRenderer.color = new Color(newValue, newValue, newValue, newValue);
            Debug.Log("Color after: " + trailerPart.spriteRenderer.color);

           // Debug.Log(alphaDecreasePerSecond * time);

            trailerPart.spriteRenderer.transform.localScale -= Vector3.one * scaleDecreasePerSecond * time;

            if (trailerPart.CustomUpdate(time))
            {
                trailerParts.Remove(trailerPart);
                Trailer.GiveTrailerPart(trailerPart);
             //   Debug.Log("Test");
            }
        }
        if ((timeLastPart -= time) <= 0.0f)
        {
            TrailerPart trailerPart = Trailer.TakeTrailerPart();
            trailerPart.Init(originalRenderer, lifeTime);
            trailerPart.transform.parent = transform;

            trailerParts.Add(trailerPart);
            timeLastPart = partDistance;
        }
    }
}