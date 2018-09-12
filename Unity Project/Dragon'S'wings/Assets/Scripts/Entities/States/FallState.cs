using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : EntityState
{
    public FallCondition fallCondition;
    public FallSave fallSave;

    public float fallTime = 0.5f;
    public float currentTimeFalling;

    public override void InitComponents()
    {
        fallCondition = GetComponentInChildren<FallCondition>();
        fallSave = GetComponentInChildren<FallSave>();
    }

    public override void EnterState()
    {
        // TODO Player Reset Hook
        entity.SetHigherLayer();
        entity.rigidbody2D.velocity = Vector2.zero;
        currentTimeFalling = 0.0f;
    }

    public override void ExecuteAction()
    {
        if (!fallCondition.EntityShouldFall())
        {
            entity.SetActionState(Entity.ActionState.Movement);
            return;
        }

        currentTimeFalling += Time.fixedDeltaTime;
        if (currentTimeFalling >= fallTime)
        {
            entity.Fall();
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
}