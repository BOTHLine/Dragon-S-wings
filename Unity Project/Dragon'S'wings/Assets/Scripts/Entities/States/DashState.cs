using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : EntityState
{
    public DashStateParameter parameter;

    public Crosshair crosshair;

    public Collider2D[] tempColliders2D;

    public bool canDash = true;
    
    public Vector2 dashTarget = Vector2.zero;
    public float distSqr;

    public float maxDashRange = 2.0f;
    public float dashSpeed = 10.0f;
    public float dashTime;
    public float dashForce = 5.0f;

    public float correctionRadius = 0.5f;

    public DashState(Entity entity) : base(entity)
    {

    }

    public override void UpdateInput()
    {
        
    }

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        parameter = (DashStateParameter)entityStateParameter;

        entity.SetHigherLayer();

        canDash = false;

        dashTarget = crosshair.transform.position;

        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(crosshair.transform.position, crosshair.circleCollider2D.radius, LayerMask.GetMask(LayerMask.LayerToName(LayerList.LevelFallingCheck)));

        if (colliders2D.Length == 0)
        {
            colliders2D = Physics2D.OverlapCircleAll(crosshair.transform.position, correctionRadius, LayerMask.GetMask(LayerMask.LayerToName(LayerList.LevelFallingCheck)));
            if (colliders2D.Length > 0)
            {
                ColliderDistance2D closestColliderDistance2D = crosshair.circleCollider2D.Distance(colliders2D[0]);
                for (int i = 1; i < colliders2D.Length; i++)
                {
                    ColliderDistance2D temp = crosshair.circleCollider2D.Distance(colliders2D[i]);
                    if (temp.distance < closestColliderDistance2D.distance)
                    {
                        closestColliderDistance2D = temp;
                    }
                }
                dashTarget = closestColliderDistance2D.pointB;
            }
        }


        dashTime = Vector2.Distance(entity.transform.position, dashTarget) / dashSpeed;
        Trailer.AddTrailer(entity.spriteRenderer, dashTime, 0.05f, 1.0f, 10.0f, 0.1f);
    }

    public override void ExecuteAction()
    {
        // entity.rigidbody2D.velocity = dashingDirection * dashSpeed;
        entity.rigidbody2D.MovePosition(Vector2.Lerp(entity.transform.position, dashTarget, dashSpeed * Time.deltaTime));

        distSqr = (dashTarget - (Vector2)entity.transform.position).sqrMagnitude;
        if (distSqr < 0.00001f)
        {
            entity.SetActionState(new FallStateParameter(new HookStateParameter()));
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

    public override void InitOwnComponents()
    {
        crosshair = entity.GetComponentInChildren<Crosshair>();
    }

    public override void InitOtherComponents()
    {
        // TODO
    }
}