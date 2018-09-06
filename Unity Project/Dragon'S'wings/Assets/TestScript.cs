using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class TestScript : MonoBehaviour
{
    public float offset;

    private void Awake()
    {
        CalculateInnerPolygonCollider2D();
    }

    private void CalculateInnerPolygonCollider2D()
    {
        PolygonCollider2D firstCollider = GetComponent<PolygonCollider2D>();
        PolygonCollider2D secondCollider = gameObject.AddComponent<PolygonCollider2D>();

        Vector2[] points = firstCollider.points;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 lastPoint = GetLastPoint(firstCollider.points, i);
            Vector2 nextPoint = GetNextPoint(firstCollider.points, i);

            Vector2 lastVector = lastPoint - firstCollider.points[i];
            Vector2 nextVector = nextPoint - firstCollider.points[i];
            
            Vector2 directionVector = (lastVector.normalized + nextVector.normalized) / 2.0f;

            float angle = Vector2.SignedAngle(lastVector, nextVector);
            if (angle > 0)
            {
                directionVector = -directionVector;
            }
            /*
             * TODO: Richtigen Fälle erkennen: Spitzer, Stumpfer und Überstumpfer Winkel und entsprechend handhaben
            if (angle > 90)
            {
                angle = 90 - angle;
            }
            points[i] = firstCollider.points[i] + (directionVector.normalized * CalculateOffset(angle));
            */
            points[i] = firstCollider.points[i] + (directionVector.normalized * offset);
        }
        secondCollider.points = points;
    }

    private Vector2 GetLastPoint(Vector2[] points, int index)
    {
        if (--index < 0)
            return points[points.Length - 1];
        return points[index];
    }

    private Vector2 GetNextPoint(Vector2[] points, int index)
    {
        if (++index >= points.Length)
            return points[0];
        return points[index];
    }

    private float CalculateOffset(float angle)
    {
        float realOffset = offset / Mathf.Sin(angle);

        return realOffset;
    }
}