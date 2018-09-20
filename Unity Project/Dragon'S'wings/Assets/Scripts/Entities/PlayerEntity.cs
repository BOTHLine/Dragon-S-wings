using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerEntity : Entity
{
    [System.Serializable]
    public struct Pull
    {

        public float pullSpeed;
        public float pullVelocity;
        public float pullMaxSpeed;
        public float pullForce;
    }

    public Hook hook;
    public Crosshair crosshair;

    public Pull pull;

    [Header("Swing")]
    public float swingSpeed;
    public float swingVelocity;
    public float swingMaxSpeed;
    public float swingForce;

    private void Update()
    {
        crosshair.UpdateInput();
        MovementState movementState = (MovementState) GetEntityState(ActionState.Movement);
        movementState.UpdateInput();
        switch (currentActionState)
        {
            case ActionState.Dash:
                break;
            case ActionState.Movement:
                if (GetEntityState(ActionState.Dash).CheckInput())
                {
                    SetActionState(new DashStateParameter(Vector2.zero)); // TODO: Get Position from Crosshair here instead of getting it inside the DashState
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
                    SetActionState(new DashStateParameter(Vector2.zero)); // TODO: Get Position from Crosshair here instead of getting it inside the DashState
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
                    SetActionState(new PullStateParameter());
                    break;
                }
                if (GetEntityState(ActionState.Swing).CheckInput())
                {
                    Vector2 swingRelativePosition = (Vector2)transform.position - hook.distanceJoint2D.connectedAnchor;
                    SwingState swingState = (SwingState) GetEntityState(ActionState.Swing);
                    SwingStateParameter swingStateParameter = new SwingStateParameter(swingSpeed, swingVelocity, swingMaxSpeed, true);
                    if (Mathf.Abs(movementState.movingDirection.x) > Mathf.Abs(movementState.movingDirection.y))
                    {
                        if (Mathf.Abs(movementState.movingDirection.x) > Mathf.Abs(movementState.movingDirection.y))
                        {
                            if (swingRelativePosition.y != 0)
                            {
                                swingStateParameter.swingClockwise = (swingRelativePosition.y > 0 && movementState.movingDirection.x > 0) || (swingRelativePosition.y < 0 && movementState.movingDirection.x < 0);
                            }
                            else
                            {
                                swingStateParameter.swingClockwise = (swingRelativePosition.x > 0 && movementState.movingDirection.y < 0) || (swingRelativePosition.x < 0 && movementState.movingDirection.y > 0);
                            }
                            SetActionState(swingStateParameter);
                        }
                        else if (Mathf.Abs(movementState.movingDirection.y) > Mathf.Abs(movementState.movingDirection.x))
                        {
                            if (swingRelativePosition.x != 0)
                            {
                                swingStateParameter.swingClockwise = (swingRelativePosition.x > 0 && movementState.movingDirection.y < 0) || (swingRelativePosition.x < 0 && movementState.movingDirection.y > 0);
                            }
                            else
                            {
                                swingStateParameter.swingClockwise = (swingRelativePosition.y > 0 && movementState.movingDirection.x > 0) || (swingRelativePosition.y < 0 && movementState.movingDirection.x < 0);
                            }
                            SetActionState(swingStateParameter);
                        }
                    }
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
                    SetActionState(new FallStateParameter(new HookStateParameter()));
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

        SetActionState(new MovementStateParameter());
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