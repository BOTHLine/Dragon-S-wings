using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangState : EntityState
{
    public HangStateParameter parameter;

    public HangState(Entity entity) : base(entity)
    {
    }

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        parameter = (HangStateParameter)entityStateParameter;
    }

    public override void ExecuteAction()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Hang;
    }

    public override void InitOtherComponents()
    {
        // TODO
    }

    public override void InitOwnComponents()
    {
        // TODO
    }
}