using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullStateParameter : EntityStateParameter
{
    public PullStateParameter()
    {
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Pull;
    }
}