using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookState : EntityState
{
    public float maxRopeLength;

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        HookStateParameter hookStateParameter = (HookStateParameter)entityStateParameter;
        entity.SetNormalLayer();
    }

    public override void ExitState()
    {
        MovementState movementState = (MovementState)entity.GetEntityState(Entity.ActionState.Movement);
        movementState.lastSavePosition = entity.transform.position;
    }

    public override void ExecuteAction()
    {
    //    throw new System.NotImplementedException();
    }

    public override bool CheckInput()
    {
        return Input.GetButtonDown(InputList.Hook);
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Hook;
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