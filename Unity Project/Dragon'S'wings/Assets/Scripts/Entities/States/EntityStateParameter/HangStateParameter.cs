using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangStateParameter : EntityStateParameter
{
    public HangStateParameter()
    {
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Hang;
    }
}