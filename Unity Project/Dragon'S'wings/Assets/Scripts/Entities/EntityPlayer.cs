using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementState))]
[RequireComponent(typeof(DashState))]
[RequireComponent(typeof(HookState))]
[RequireComponent(typeof(FallState))]
[RequireComponent(typeof(PushState))]
[RequireComponent(typeof(HangState))]
[RequireComponent(typeof(PullState))]
[RequireComponent(typeof(SwingState))]
[RequireComponent(typeof(DistanceJoint2D))]
public class EntityPlayer : Entity
{
    public DistanceJoint2D distanceJoint2D;

    public Crosshair crosshair;

    public Vector2 lastSavePosition;

    public override void InitComponents()
    {
        distanceJoint2D = GetComponent<DistanceJoint2D>();

        crosshair = GetComponentInChildren<Crosshair>();
    }

    private void Update()
    {
        crosshair.UpdateInput();
        EntityState movement = GetEntityState(ActionState.Movement);
        movement.UpdateInput();
        switch (currentActionState)
        {
            case ActionState.Dash:
                break;
            case ActionState.Movement:
                if (GetEntityState(ActionState.Dash).CheckInput())
                {
                    SetActionState(ActionState.Dash);
                    break;
                }
                if (GetEntityState(ActionState.Hook).CheckInput())
                {

                }
                break;
            case ActionState.Fall:
                GetEntityState(ActionState.Dash).CheckInput();
                break;
            case ActionState.Hook:
                GetEntityState(ActionState.Pull).CheckInput();
                GetEntityState(ActionState.Swing).CheckInput();
                break;
            case ActionState.Pull:
                break;
            case ActionState.Swing:
                break;
            case ActionState.Push:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentActionState)
        {
            case ActionState.Dash:
                break;
            case ActionState.Fall:
                break;
            case ActionState.Hook:
                GetEntityState(ActionState.Movement).ExecuteAction();
                break;
            case ActionState.Movement:
                break;
            case ActionState.Pull:
                break;
            case ActionState.Push:
                break;
            case ActionState.Swing:
                break;
        }
        GetEntityState(currentActionState).ExecuteAction();
    }

    public override void Fall()
    {
        transform.position = lastSavePosition;
    }

    public override void SetHigherLayer()
    {
        gameObject.layer = LayerList.PlayerDashing;
    }

    public override void SetNormalLayer()
    {
        gameObject.layer = LayerList.Player;
    }
}