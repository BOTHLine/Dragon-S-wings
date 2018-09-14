using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class FallCondition : MonoBehaviour
{
    FallState fallState;

    public CircleCollider2D circleCollider2D;

    private void Awake()
    {
        fallState = GetComponentInParent<FallState>();
        
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;
    }

    private void Start()
    {
        transform.localPosition = GetComponentInParent<Entity>().circleCollider2D.offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject.layer == LayerList.LevelFallingCheck)
        {
            Debug.Log("Triggered");
            Vector2 closestContactPoint = collision.contacts[0].point;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Distance(transform.position, contact.point) < Vector2.Distance(transform.position, closestContactPoint))
                {
                    closestContactPoint = contact.point;
                }
            }
            Vector2 collisionVector = closestContactPoint - (Vector2)transform.position;
            fallState.SaveEntity(collisionVector);
            circleCollider2D.enabled = false;
        }
    }
}