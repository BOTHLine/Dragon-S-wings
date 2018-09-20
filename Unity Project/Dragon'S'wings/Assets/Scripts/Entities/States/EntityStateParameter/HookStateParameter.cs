using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookStateParameter : EntityStateParameter
{
    public HookStateParameter()
    {
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Hook;
    }
}