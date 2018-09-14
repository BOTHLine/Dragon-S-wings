using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : EntityState
{
    public DashState dashState;
    public CircleCollider2D circleCollider2D;

    public float fallTime = 0.5f;
    public float currentTimeFalling;
    public bool isFalling;

    public Entity.ActionState targetActionState;

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        FallStateParameter fallStateParameter = (FallStateParameter)entityStateParameter;
        targetActionState = fallStateParameter.targetActionState;

        // TODO Player Reset Hook
        entity.SetNormalLayer();
        entity.rigidbody2D.velocity = Vector2.zero;
        currentTimeFalling = 0.0f;

        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(entity.transform.position, circleCollider2D.radius, LayerMask.GetMask(LayerMask.LayerToName(LayerList.LevelFallingCheck)));
        isFalling = colliders2D.Length < 1;
    }

    public override void ExecuteAction()
    {
        if (!isFalling)
        {
            entity.SetActionState(targetActionState, null);
            return;
        }
        if ( !dashState || !dashState.canDash || currentTimeFalling >= fallTime)
        {
            entity.Fall();
            currentTimeFalling += Time.fixedDeltaTime;
        }
    }

    public override void ExitState()
    {
    //    throw new System.NotImplementedException();
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Fall;
    }

    public override void InitOtherComponents()
    {
        dashState = (DashState) entity.GetEntityState(Entity.ActionState.Dash);
    }

    public override void InitOwnComponents()
    {
        circleCollider2D = entity.GetComponent<CircleCollider2D>();
    }

    public void SaveEntity(Vector2 collisionVector)
    {
        Debug.Log("Entity Save");
        Debug.Log(collisionVector);
        DashState dashState = (DashState) entity.GetEntityState(Entity.ActionState.Dash);
        //   entity.Push(dashState.dashSpeed, collisionVector.normalized * collisionVector.magnitude);
        entity.Fall();
    }
}