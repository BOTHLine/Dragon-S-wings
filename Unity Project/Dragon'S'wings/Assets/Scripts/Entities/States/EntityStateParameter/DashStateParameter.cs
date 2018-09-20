using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStateParameter : EntityStateParameter
{
    public Vector2 dashTarget;

    public DashStateParameter(Vector2 dashTarget)
    {
        this.dashTarget = dashTarget;
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Dash;
    }
}