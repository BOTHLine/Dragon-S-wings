using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Island : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        Vector2[] points = new Vector2[polygonCollider.points.Length + 1];
        for (int i = 0; i < polygonCollider.points.Length; i++)
        {
            points[i] = polygonCollider.points[i];
        }
        points[points.Length - 1] = points[0];
        edgeCollider.points = points;
    }
}