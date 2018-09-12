using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : EntityState
{
    public Crosshair crosshair;

    public bool canDash = true;
    
    public Vector2 dashingDirection = Vector2.zero;

    public float maxDashRange = 2.0f;
    public float dashSpeed = 10.0f;
    public float dashTime;
    public float currentTimeDashing;
    public float dashForce = 5.0f;

    public override void UpdateInput()
    {

        // TODO: Handle Highlighting, combine with Hook!
        /*
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, crosshair.localPosition, player.hook.maxRopeLength, player.hook.layerMask);
        if (raycastHit2D.collider)
        {
            Debug.DrawLine(transform.position, raycastHit2D.point);
            if (raycastHit2D.distance < crosshairPosition.magnitude)
            {
                crosshairPosition = crosshairPosition.normalized * raycastHit2D.distance;
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
        */
    }

    public override void InitComponents()
    {
        crosshair = GetComponentInChildren<Crosshair>();
    }

    public override void EnterState()
    {
        entity.SetHigherLayer();

        canDash = false;
        currentTimeDashing = 0.0f;

        dashTime = crosshair.aimingVector.magnitude / dashSpeed;
        dashingDirection = crosshair.aimingVector.normalized;
        Trailer.AddTrailer(entity.spriteRenderer, dashTime, 0.05f, 1.0f, 10.0f, 0.1f);
    }

    public override void ExecuteAction()
    {
        entity.rigidbody2D.velocity = dashingDirection * dashSpeed;

        currentTimeDashing += Time.fixedDeltaTime;
        if (currentTimeDashing >= dashTime)
        {
            entity.rigidbody2D.velocity = Vector2.zero;
            entity.SetActionState(Entity.ActionState.Fall);
        }
    }

    public override bool CheckInput()
    {
        return (canDash && Input.GetButton(InputList.Dash));
    }

    public override void ExitState()
    {

    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Dash;
    }
}