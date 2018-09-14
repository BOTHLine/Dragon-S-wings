using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStateParameter : EntityStateParameter
{
    public Entity.ActionState targetActionState;
    // public EntityStateParameter targetEntityStateParameter       Dieser Parameter kann dann später beim Ändern wieder übergeben werden

    public FallStateParameter(Entity.ActionState targetActionState)
    {
        this.targetActionState = targetActionState;
    }
}