using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : EntityState
{
    public Vector2 movingDirection = Vector2.zero;
    public float movementSpeed = 5.0f;
    
    public Vector2 aimingDirection = Vector2.zero;

    public override void EnterState()
    {
    //    throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
    //    throw new System.NotImplementedException();
    }

    public override void UpdateInput()
    {
        // Movement
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

    public override void InitComponents()
    {
        
    }
}