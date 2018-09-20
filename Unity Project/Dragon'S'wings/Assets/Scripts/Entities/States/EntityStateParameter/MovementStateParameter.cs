using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateParameter : EntityStateParameter
{
    public MovementStateParameter()
    {
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Movement;
    }
}