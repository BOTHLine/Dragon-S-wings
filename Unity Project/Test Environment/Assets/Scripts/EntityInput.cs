using UnityEngine;

public abstract class EntityInput
{
    public abstract void ReadInput();

    public event System.Action<Vector2> OnMoveInput = delegate { };
    public event System.Action<Vector2> OnDashInput = delegate { };

    protected void RaiseOnMoveInput(Vector2 moveDirection)
    {
        OnMoveInput(moveDirection);
    }

    protected void RaiseOnDashInputEvent(Vector2 dashTargetPosition)
    {
        OnDashInput(dashTargetPosition);
    }
}