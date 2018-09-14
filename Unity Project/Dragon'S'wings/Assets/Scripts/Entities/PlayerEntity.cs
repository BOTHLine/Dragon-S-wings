using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SwingState))]
[RequireComponent(typeof(HangState))]
[RequireComponent(typeof(PullState))]
[RequireComponent(typeof(HookState))]
[RequireComponent(typeof(FallState))]
[RequireComponent(typeof(PushState))]
[RequireComponent(typeof(DashState))]
[RequireComponent(typeof(MovementState))]
public class PlayerEntity : Entity
{
    public Hook hook;
    public Crosshair crosshair;

    private void Update()
    {
        crosshair.UpdateInput();
        EntityState movementState = GetEntityState(ActionState.Movement);
        movementState.UpdateInput();
        switch (currentActionState)
        {
            case ActionState.Dash:
                break;
            case ActionState.Movement:
                if (GetEntityState(ActionState.Dash).CheckInput())
                {
                    SetActionState(ActionState.Dash, new DashStateParameter(Vector2.zero)); // TODO: Get Position from Crosshair here instead of getting it inside the DashState
                    break;
                }
                if (GetEntityState(ActionState.Hook).CheckInput())
                {
                    hook.Shoot(crosshair.transform.localPosition);
                }
                break;
            case ActionState.Fall:
                if (GetEntityState(ActionState.Dash).CheckInput())
                {
                    SetActionState(ActionState.Dash, new DashStateParameter(Vector2.zero)); // TODO: Get Position from Crosshair here instead of getting it inside the DashState
                }
                break;
            case ActionState.Hook:
                if (Input.GetButtonDown(InputList.Release))
                {
                    hook.ResetAnchorPoints();
                    break;
                }
                if (GetEntityState(ActionState.Pull).CheckInput())
                {
                    SetActionState(ActionState.Pull, new PullStateParameter());
                    break;
                }
                if (GetEntityState(ActionState.Swing).CheckInput())
                {

                }
                break;
            case ActionState.Pull:
                if (Input.GetButtonDown(InputList.Release))
                {
                    hook.ResetAnchorPoints();
                    break;
                }
                if (GetEntityState(ActionState.Hook).CheckInput())
                {
                    SetActionState(ActionState.Fall, new FallStateParameter(ActionState.Hook));
                }
                break;
            case ActionState.Swing:
                if (Input.GetButtonDown(InputList.Release))
                {
                    hook.ResetAnchorPoints();
                    break;
                }
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
        MovementState movementState = (MovementState)GetEntityState(ActionState.Movement);
        transform.position = movementState.lastSavePosition;

        SetActionState(ActionState.Movement, new MovementStateParameter());
    }

    public override void SetHigherLayer()
    {
        gameObject.layer = LayerList.PlayerHigher;
    }

    public override void SetNormalLayer()
    {
        gameObject.layer = LayerList.Player;
    }

    public override Vector2 GetAimingVector()
    {
        return crosshair.transform.localPosition;
    }

    public override void InitOtherComponents()
    {

    }

    public override void InitOwnComponents()
    {
        hook = GetComponentInChildren<Hook>();
        crosshair = GetComponentInChildren<Crosshair>();
    }
}