using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : EntityState
{
    public MovementStateParameter parameter;

    public Vector2 movingDirection = Vector2.zero;
    public float movementSpeed = 5.0f;

    public Vector2 lastSavePosition;

    public MovementState(Entity entity) : base(entity)
    {
    }

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        parameter = (MovementStateParameter)entityStateParameter;
        entity.SetNormalLayer();

        DashState dashState = (DashState) entity.GetEntityState(Entity.ActionState.Dash);
        dashState.canDash = true;
    }

    public override void ExitState()
    {
        lastSavePosition = entity.transform.position;
    }

    public override void UpdateInput()
    {
        movingDirection.x = Input.GetAxisRaw(InputList.Horizontal);
        movingDirection.y = Input.GetAxisRaw(InputList.Vertical);

        movingDirection = movingDirection.normalized;
    }

    public override void ExecuteAction()
    {
        entity.rigidbody2D.velocity = movingDirection * movementSpeed;
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Movement;
    }

    public override void InitOwnComponents()
    {
        // TODO
    }

    public override void InitOtherComponents()
    {
        // TODO
    }
}