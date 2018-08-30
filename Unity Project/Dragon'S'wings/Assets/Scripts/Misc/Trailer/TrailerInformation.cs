using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerInformation : MonoBehaviour
{
    public SpriteRenderer originalRenderer;
    public float duration;
    public float time = 0.0f;
    public float timeLastPart = 0.0f;
    public float partDistance;
    public float lifeTime;
    public float alphaDecreasePerSecond;
    public float scaleDecreasePerSecond;
    public List<TrailerPart> trailerParts = new List<TrailerPart>();

    public void Init(SpriteRenderer originalRenderer, float duration, float partDistance, float lifeTime, float alphaDecreasePerSecond, float scaleDecreasePerSecond)
    {
        this.originalRenderer = originalRenderer;
        this.duration = duration;
        this.partDistance = partDistance;
        timeLastPart = partDistance;
        this.lifeTime = lifeTime;
        this.alphaDecreasePerSecond = alphaDecreasePerSecond;
        this.scaleDecreasePerSecond = scaleDecreasePerSecond;
    }

    public bool CustomUpdate(float time)
    {
        for (int i = 0; i < trailerParts.Count; i++)
        {
            TrailerPart trailerPart = trailerParts[i];

            trailerPart.spriteRenderer.transform.localScale -= Vector3.one * scaleDecreasePerSecond * time;

            if (trailerPart.CustomUpdate(time))
            {
                trailerParts.Remove(trailerPart);
                Trailer.GiveTrailerPart(trailerPart);
                Debug.Log("Test");
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
        if ((duration -= time) <= 0.0f)
            return true;
        return false;
    }
}