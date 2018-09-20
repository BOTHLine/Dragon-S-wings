using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingStateParameter : EntityStateParameter
{
    public float swingSpeed;
    public float swingVelocity;
    public float swingMaxSpeed;
    public bool swingClockwise;

    public SwingStateParameter(float swingSpeed, float swingVelocity, float swingMaxSpeed, bool swingClockwise)
    {
        this.swingSpeed = swingSpeed;
        this.swingVelocity = swingVelocity;
        this.swingMaxSpeed = swingMaxSpeed;
        this.swingClockwise = swingClockwise;
    }

    public override Entity.ActionState GetActionState()
    {
        return Entity.ActionState.Swing;
    }
}