using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Crosshair : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public SpriteRenderer spriteRenderer;

    public DashState dashState;
    public HookState hookState;

    public Hook hook;

    public Vector2 aimingVector { get { return transform.localPosition; } }

    public TreeHighlighter currentHighlighted;
    
    private void Awake()
    {
        InitComponents();
    }

    private void InitComponents()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        dashState = GetComponentInParent<DashState>();
        hookState = GetComponentInParent<HookState>();

        hook = FindObjectOfType<Hook>();
    }
    
    public void UpdateInput()
    {
        CalculateCrosshairPosition();
        CalculateHighlighting();
    }

    public void CalculateCrosshairPosition()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 aimDirection = worldMousePosition - transform.parent.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        if (aimAngle < 0.0f)
            aimAngle = Mathf.PI * 2 + aimAngle;

        Vector3 newPosition = Quaternion.Euler(0.0f, 0.0f, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        transform.localPosition = newPosition.normalized * Mathf.Min(aimDirection.magnitude, dashState.maxDashRange);
    }

    public void CalculateHighlighting()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.parent.position, aimingVector, hookState.maxRopeLength, hook.layerMask);
        if (raycastHit2D.collider)
        {
            Debug.DrawLine(transform.parent.position, raycastHit2D.point);
            if (raycastHit2D.distance < aimingVector.magnitude)
            {
                transform.localPosition = aimingVector.normalized * raycastHit2D.distance;
            }
            TreeHighlighter test = raycastHit2D.collider.GetComponent<TreeHighlighter>();
            if (test)
            {
                if (currentHighlighted)
                {
                    if (test != currentHighlighted)
                    {
                        currentHighlighted.SetHighlight(false);
                        test.SetHighlight(true);
                        currentHighlighted = test;
                    }
                }
                else
                {
                    test.SetHighlight(true);
                    currentHighlighted = test;
                }
            }
            else if (currentHighlighted)
            {
                currentHighlighted.SetHighlight(false);
                currentHighlighted = null;
            }
        }
        else if (currentHighlighted)
        {
            currentHighlighted.SetHighlight(false);
            currentHighlighted = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerList.LevelFallingCheck)
        {
            Vector2 closestContactPoint = collision.contacts[0].point;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Distance(transform.position, contact.point) < Vector2.Distance(transform.position, closestContactPoint))
                {
                    closestContactPoint = contact.point;
                }
            }
            Vector2 collisionVector = closestContactPoint - (Vector2)transform.position;
        }
    }
}