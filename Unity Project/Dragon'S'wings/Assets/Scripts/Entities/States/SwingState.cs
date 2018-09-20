using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingState : EntityState
{
    public SwingStateParameter parameter;

    public Hook hook;

    public SwingState(Entity entity) : base(entity)
    {
    }

    public override bool CheckInput()
    {
        MovementState movementState = (MovementState)entity.GetEntityState(Entity.ActionState.Movement);
        return Input.GetButtonDown(InputList.Swing) && movementState.movingDirection.Equals(Vector2.zero);
    }

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        parameter = (SwingStateParameter)entityStateParameter;
        entity.SetHigherLayer();
    }

    public override void ExecuteAction()
    {
        if (!Input.GetButton(InputList.Swing))
        {
            entity.SetActionState(new FallStateParameter(new MovementStateParameter())); // TODO Nach dem Schwingen Seil lösen oder behalten
            return;
        }
        Vector2 centerVector = hook.distanceJoint2D.connectedAnchor - (Vector2)entity.transform.position;
        Vector2 swingVector = parameter.swingClockwise ? new Vector2(-centerVector.y, centerVector.x) : new Vector2(centerVector.y, -centerVector.x);
        hook.distanceJoint2D.distance = centerVector.magnitude;
        if (parameter.swingVelocity == 0.0f)
        {
            entity.rigidbody2D.velocity = swingVector.normalized * parameter.swingSpeed;
        }
        else
        {
            entity.rigidbody2D.AddForce(swingVector.normalized * parameter.swingVelocity);
            if (entity.rigidbody2D.velocity.magnitude > parameter.swingSpeed)
            {
                entity.rigidbody2D.velocity = entity.rigidbody2D.velocity.normalized * parameter.swingSpeed;
            }
        }
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Swing;
    }

    public override void InitOtherComponents()
    {
        // TODO
    }

    public override void InitOwnComponents()
    {
        hook = entity.GetComponentInChildren<Hook>();
    }
}