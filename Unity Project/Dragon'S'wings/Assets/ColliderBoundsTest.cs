using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBoundsTest : MonoBehaviour
{
    private new CircleCollider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        Vector2[] closestPoints = new Vector2[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            ColliderDistance2D colliderDistance2D = colliders[i].Distance(collider2D);
            //closestPoints[i] = colliderDistance2D.pointA;
            Debug.DrawLine(colliderDistance2D.pointA, colliderDistance2D.pointB);
            // Point A Position auf dem eigenen Collider, der .Distance aufruft
            // Point B Position auf dem anderen Collider, der als Parameter übergeben wird 
        }
    }

    private void Update()
    {
        /*
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }
        */
    }
}