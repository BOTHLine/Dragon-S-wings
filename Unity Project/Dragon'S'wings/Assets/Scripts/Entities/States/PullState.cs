using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullState : EntityState
{
    public float pullSpeed = 5.0f;
    public float pullVelocity = 0.0f;

    public Hook.AnchorPoint currentTargetAnchorPoint;
    public Vector2 pullTarget = Vector2.zero;

    public Hook hook;
    public DistanceJoint2D distanceJoint2D;

    public override void EnterState(EntityStateParameter entityStateParameter)
    {
        PullStateParameter pullStateParameter = (PullStateParameter)entityStateParameter;
        entity.SetHigherLayer();
        CalculateNextPullTarget();
    }

    // TODO: Position weiter als den Connected Anchor setzen. Dafür die Distanzverringerung im Hook auch beschränken, dass der DistanceJoint2D minimal eine Distanz von dieser Erweiterung beträgt (CircleCollider2D.radius?)
    public override void ExecuteAction()
    {
        if (!currentTargetAnchorPoint.Equals(hook.GetLastAnchorPoint()))
        {
            CalculateNextPullTarget();
        }
        Debug.DrawLine(transform.position, pullTarget);

        if (hook.anchorPoints.Count == 1 && Mathf.Abs((distanceJoint2D.connectedAnchor - (Vector2)entity.transform.position).sqrMagnitude - entity.circleCollider2D.radius * entity.circleCollider2D.radius) <= 0.1f)
        {
            hook.RemoveLastAnchorPoint();
            return;
        }

        Vector2 pullVector = pullTarget - (Vector2)transform.position;
        Vector2 pullDirection = pullVector.normalized;
        if (pullVelocity == 0.0f)
        {
            entity.rigidbody2D.velocity = pullDirection * pullSpeed;
        }
        else
        {
            entity.rigidbody2D.AddForce(pullDirection * pullVelocity);
            if (entity.rigidbody2D.velocity.sqrMagnitude > pullSpeed * pullSpeed)
            {
                entity.rigidbody2D.velocity = entity.rigidbody2D.velocity.normalized * pullSpeed;
            }
        }

        float pullSqrMagnitude = pullVector.sqrMagnitude;
        if (pullSqrMagnitude < distanceJoint2D.distance * distanceJoint2D.distance)
        {
            distanceJoint2D.distance = Mathf.Min(pullVector.magnitude, entity.circleCollider2D.radius);
        }
    }

    public override void ExitState()
    {
        // Empty
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Pull;
    }

    public override bool CheckInput()
    {
        return !Input.GetButton(InputList.Hook);
    }

    public override void InitOwnComponents()
    {
        hook = FindObjectOfType<Hook>();
    }

    public override void InitOtherComponents()
    {
        distanceJoint2D = entity.GetComponent<DistanceJoint2D>();
    }

    public void CalculateNextPullTarget()
    {
        currentTargetAnchorPoint = hook.GetLastAnchorPoint();
        if (hook.anchorPoints.Count == 1)
        {
            pullTarget = distanceJoint2D.connectedAnchor;
        }
        else
        {
            pullTarget = distanceJoint2D.connectedAnchor + (distanceJoint2D.connectedAnchor - (Vector2)entity.transform.position).normalized * entity.circleCollider2D.radius;
        }
        Debug.Log("Next Pull Target: " + pullTarget);
    }
}