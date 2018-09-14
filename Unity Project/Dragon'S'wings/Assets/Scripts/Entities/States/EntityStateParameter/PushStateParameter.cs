using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStateParameter : EntityStateParameter
{
    public Vector2 pushVector;
    public float pushSpeed;

    public PushStateParameter(Vector2 pushVector, float pushSpeed)
    {
        this.pushVector = pushVector;
        this.pushSpeed = pushSpeed;
    }
}