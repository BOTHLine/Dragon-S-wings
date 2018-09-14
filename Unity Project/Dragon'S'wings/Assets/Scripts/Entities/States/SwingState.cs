using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingState : EntityState
{
    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        SwingStateParameter swingStateParameter = (SwingStateParameter)entityStateParameter;
        entity.SetHigherLayer();
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
        return Entity.ActionState.Swing;
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