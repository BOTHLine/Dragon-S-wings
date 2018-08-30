using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    public static Dictionary<SpriteRenderer, TrailerInformation> trailerInformations = new Dictionary<SpriteRenderer, TrailerInformation>();
    public static List<TrailerPart> trailerPartPool = new List<TrailerPart>();

    private static Trailer trailer;
    private static GameObject pool;

    private void Awake()
    {
        trailer = this;

        pool = new GameObject("Pool");
        pool.transform.parent = transform;
    }

    private void Update()
    {
        SpriteRenderer[] allSpriteRenderer = new SpriteRenderer[trailerInformations.Keys.Count];
        trailerInformations.Keys.CopyTo(allSpriteRenderer, 0);

        for (int i = 0; i < allSpriteRenderer.Length; i++)
        {
            SpriteRenderer spriteRenderer = allSpriteRenderer[i];
            TrailerInformation trailerInformation;
            trailerInformations.TryGetValue(spriteRenderer, out trailerInformation);

            if (trailerInformation.CustomUpdate(Time.deltaTime))
            {
                trailerInformations.Remove(trailerInformation.originalRenderer);
                for (int j = 0; j < trailerInformation.trailerParts.Count; j++)
                {
                    TrailerPart trailerPart = trailerInformation.trailerParts[j];
                    GiveTrailerPart(trailerPart);
                }
                Destroy(trailerInformation.gameObject);
            }
        }
	}

    public static void AddTrailer(SpriteRenderer spriteRenderer, float duration, float partDistance, float lifeTime, float alphaDecreasePerSecond, float scaleDecreasePerSecond)
    {
        if (trailerInformations.ContainsKey(spriteRenderer))
        {
            Debug.Log("TrailRenderer für diesen SpriteRenderer bereits aktiv.");
        }
        else
        {
            GameObject newGameObject = new GameObject(spriteRenderer.gameObject.name);
            newGameObject.transform.parent = trailer.transform;

            TrailerInformation trailerInformation = newGameObject.AddComponent<TrailerInformation>();
            trailerInformation.Init(spriteRenderer, duration, partDistance, lifeTime, alphaDecreasePerSecond, scaleDecreasePerSecond);

            trailerInformations.Add(spriteRenderer, trailerInformation);
        }
    }

    public static void RemoveTrailer(SpriteRenderer spriteRenderer)
    {
        trailerInformations.Remove(spriteRenderer);
    }

    public static TrailerPart TakeTrailerPart()
    {
        TrailerPart trailerPart;
        if (trailerPartPool.Count > 0)
        {
            trailerPart = trailerPartPool[0];
            trailerPartPool.Remove(trailerPart);
            trailerPart.gameObject.SetActive(true);
        } else
        {
            GameObject gameObject = new GameObject("Trailer Part");
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            trailerPart = gameObject.AddComponent<TrailerPart>();
            trailerPart.spriteRenderer = spriteRenderer;
        }
        return trailerPart;
    }

    public static void GiveTrailerPart(TrailerPart trailerPart)
    {
        trailerPartPool.Add(trailerPart);
        trailerPart.transform.parent = pool.transform;
        trailerPart.gameObject.SetActive(false);
    }
}