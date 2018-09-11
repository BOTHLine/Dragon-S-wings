using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public DashState dashState;
    public Hook hook;

    public Vector2 aimingVector { get { return transform.localPosition; } }

    public TreeHighlighter currentHighlighted;

    private void Awake()
    {
        InitComponents();
    }

    private void InitComponents()
    {
        dashState = GetComponentInParent<DashState>();
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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.parent.position, aimingVector, hook.maxRopeLength, hook.layerMask);
        if (raycastHit2D.collider)
        {
            Debug.DrawLine(transform.position, raycastHit2D.point);
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
}