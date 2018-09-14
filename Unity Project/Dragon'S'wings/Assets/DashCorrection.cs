using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DashCorrection : MonoBehaviour
{
    public Dictionary<Collider2D, Vector2> closestCollisionPoints = new Dictionary<Collider2D, Vector2>();
    public Vector2[] collisions;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 closestCollisionPoint = collision.contacts[0].point;
        for (int i = 1; i < collision.contacts.Length; i++)
        {
            if (Vector2.Distance(transform.position, collision.contacts[i].point) < Vector2.Distance(transform.position, closestCollisionPoint))
            {
                closestCollisionPoint = collision.contacts[i].point;
            }
        }
        closestCollisionPoints.Add(collision.collider, closestCollisionPoint);

        collisions = new Vector2[closestCollisionPoints.Count];
        closestCollisionPoints.Values.CopyTo(collisions, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 closestCollisionPoint = collision.contacts[0].point;
        for (int i = 1; i < collision.contacts.Length; i++)
        {
            if (Vector2.Distance(transform.position, collision.contacts[i].point) < Vector2.Distance(transform.position, closestCollisionPoint))
            {
                closestCollisionPoint = collision.contacts[i].point;
            }
        }
        closestCollisionPoints.Remove(collision.collider);
        closestCollisionPoints.Add(collision.collider, closestCollisionPoint);

        collisions = new Vector2[closestCollisionPoints.Count];
        closestCollisionPoints.Values.CopyTo(collisions, 0);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        closestCollisionPoints.Remove(collision.collider);
        collisions = new Vector2[closestCollisionPoints.Count];
        closestCollisionPoints.Values.CopyTo(collisions, 0);
    }

    public Vector2 GetCorrectedPosition()
    {
        Vector2[] collisionPoints = new Vector2[closestCollisionPoints.Count];
        closestCollisionPoints.Values.CopyTo(collisionPoints, 0);

        if (collisionPoints.Length == 0)
        {
            return Vector2.positiveInfinity;
        }

        Vector2 correctedPosition = collisionPoints[0];
        for (int i = 1; i < collisionPoints.Length; i++)
        {
            if (Vector2.Distance(transform.position, collisionPoints[i]) < Vector2.Distance(transform.position, correctedPosition))
            {
                correctedPosition = collisionPoints[i];
            }
        }
        return correctedPosition;
    }
}