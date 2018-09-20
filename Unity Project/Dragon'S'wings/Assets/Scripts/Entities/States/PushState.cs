using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : EntityState
{
    PushStateParameter parameter;

    public float pushSpeed;
    public Vector2 pushVector;
    public float pushTime;
    public float currentTimePushing;

    public PushState(Entity entity) : base(entity)
    {
    }

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        parameter = (PushStateParameter)entityStateParameter;
        entity.SetHigherLayer();

        currentTimePushing = 0.0f;
    }

    public override void ExecuteAction()
    {
        entity.rigidbody2D.velocity = pushVector.normalized * pushSpeed;

        currentTimePushing += Time.fixedDeltaTime;
        if (currentTimePushing >= pushTime)
        {
            entity.rigidbody2D.velocity = Vector2.zero;
            entity.SetActionState(new FallStateParameter(parameter.targetEntityStateParameter));
        }
    }

    public override void ExitState()
    {
        // Empty
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Push;
    }

    public override bool CheckInput()
    {
        throw new System.NotImplementedException();
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