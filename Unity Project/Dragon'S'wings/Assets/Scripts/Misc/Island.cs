using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Island : MonoBehaviour
{
    private static readonly float offset = 0.1f;

    private EdgeCollider2D edgeCollider2D;
    private PolygonCollider2D polygonTrigger2D;

    private void Awake()
    {
        gameObject.layer = LayerList.LevelTrigger;

        GameObject edgeColliderObject = new GameObject("EdgeColliderObject");
        edgeColliderObject.transform.parent = transform;
        edgeColliderObject.layer = LayerList.LevelEdge;
        edgeColliderObject.transform.localPosition = Vector3.zero;
        edgeColliderObject.transform.localScale = Vector3.one;

        PolygonCollider2D originalPolygonCollider2D = GetComponent<PolygonCollider2D>();

        edgeCollider2D = edgeColliderObject.AddComponent<EdgeCollider2D>();

        CreateOuterEdgeCollider2D(originalPolygonCollider2D);

        polygonTrigger2D = gameObject.AddComponent<PolygonCollider2D>();
        polygonTrigger2D.isTrigger = true;
        CreateInnerPolygonTrigger2D(originalPolygonCollider2D);

        Destroy(originalPolygonCollider2D);
    }

    private void CreateOuterEdgeCollider2D(PolygonCollider2D originalPolygonCollider2D)
    {
        Vector2[] points = new Vector2[originalPolygonCollider2D.points.Length + 1];
        for (int i = 0; i < originalPolygonCollider2D.points.Length; i++)
        {
            points[i] = originalPolygonCollider2D.points[i];
        }
        points[points.Length - 1] = points[0];
        edgeCollider2D.points = points;
    }

    private void CreateInnerPolygonTrigger2D(PolygonCollider2D originalPolygonCollider2D)
    {
        Vector2[] points = originalPolygonCollider2D.points;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 lastPoint = GetLastPoint(originalPolygonCollider2D.points, i);
            Vector2 nextPoint = GetNextPoint(originalPolygonCollider2D.points, i);

            Vector2 lastVector = lastPoint - originalPolygonCollider2D.points[i];
            Vector2 nextVector = nextPoint - originalPolygonCollider2D.points[i];

            Vector2 directionVector = (lastVector.normalized + nextVector.normalized) / 2.0f;

            float angle = Vector2.SignedAngle(lastVector, nextVector);
            if (angle < 0)
            {
                directionVector = -directionVector;
            }

            points[i] = originalPolygonCollider2D.points[i] + (directionVector.normalized * offset);
        }
        polygonTrigger2D.points = points;
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
}