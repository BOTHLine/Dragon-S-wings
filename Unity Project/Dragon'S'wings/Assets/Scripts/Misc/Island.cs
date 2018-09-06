﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Island : MonoBehaviour
{
    public float offset = 0.1f;

    private PolygonCollider2D polygonCollider;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        GameObject edgeColliderObject = new GameObject();
        edgeColliderObject.transform.parent = transform;
        edgeColliderObject.layer = LayerList.LevelEdge;

        polygonCollider = GetComponent<PolygonCollider2D>();
        edgeCollider = edgeColliderObject.AddComponent<EdgeCollider2D>();

        Vector2[] points = new Vector2[polygonCollider.points.Length + 1];
        for (int i = 0; i < polygonCollider.points.Length; i++)
        {
            points[i] = polygonCollider.points[i];
        }
        points[points.Length - 1] = points[0];
        edgeCollider.points = points;
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
}