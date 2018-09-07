using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Enable this object only when needed, then OnCollisionEnter2D will fire right away. Use the collision information to determine where the closest hit
// Use rigidbody2D.MovePosition to move instead of interacting with transform directly
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingSave : MonoBehaviour
{
    private Character character;

    private new Collider2D collider2D;

    public float repositioningSpeed = 10.0f;

    private void Start()
    {
        character = GetComponentInParent<Character>();

        collider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 closestContactPoint = collision.contacts[0].point;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Distance(contact.point, (Vector2) transform.position + collider2D.offset) < Vector2.Distance(closestContactPoint, (Vector2) transform.position + collider2D.offset))
            {
                closestContactPoint = contact.point;
            }
        }
        Vector2 vectorToCollision = closestContactPoint - (Vector2)character.transform.position;
        character.StartRepositioning(repositioningSpeed, vectorToCollision.normalized * (vectorToCollision.magnitude + 0.5f)); // Magic Number: Threshold to go over the edge collider itself

        Debug.Log("Entered");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 closestContactPoint = collision.contacts[0].point;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Distance(contact.point, (Vector2)transform.position + collider2D.offset) < Vector2.Distance(closestContactPoint, (Vector2)transform.position + collider2D.offset))
            {
                closestContactPoint = contact.point;
            }
        }
        Vector2 vectorToCollision = closestContactPoint - (Vector2)character.transform.position;
        character.StartRepositioning(repositioningSpeed, vectorToCollision.normalized * (vectorToCollision.magnitude + 0.5f)); // Magic Number: Threshold to go over the edge collider itself

        Debug.Log("Stayed");
    }
}