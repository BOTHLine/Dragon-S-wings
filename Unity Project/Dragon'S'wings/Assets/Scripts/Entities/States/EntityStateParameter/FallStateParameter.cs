using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStateParameter : EntityStateParameter
{
    public EntityStateParameter targetEntityStateParameter;
    // public EntityStateParameter targetEntityStateParameter       Dieser Parameter kann dann später beim Ändern wieder übergeben werden

    public FallStateParameter(EntityStateParameter targetEntityStateParameter)
    {
        this.targetEntityStateParameter = targetEntityStateParameter;
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Fall;
    }
}