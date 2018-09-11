using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : EntityState
{
    public float pushSpeed;
    public Vector2 pushVector;
    public float pushTime;
    public float timePushing;

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteAction()
    {
        entity.rigidbody2D.velocity = pushVector.normalized * pushSpeed;

        timePushing += Time.fixedDeltaTime;
        if (timePushing >= pushTime)
        {
            entity.rigidbody2D.velocity = Vector2.zero;
            entity.SetActionState(Entity.ActionState.Fall);
        }
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Push;
    }

    public override bool CheckInput()
    {
        throw new System.NotImplementedException();
    }

    public override void InitComponents()
    {
        throw new System.NotImplementedException();
    }
}