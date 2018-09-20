using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStateParameter : EntityStateParameter
{
    public EntityStateParameter targetEntityStateParameter;

    public Vector2 pushVector;
    public float pushSpeed;

    public PushStateParameter(EntityStateParameter targetEntityStateParameter, Vector2 pushVector, float pushSpeed)
    {
        this.targetEntityStateParameter = targetEntityStateParameter;
        this.pushVector = pushVector;
        this.pushSpeed = pushSpeed;
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Push;
    }
}