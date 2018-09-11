using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Entity : MonoBehaviour
{
    public enum ActionState
    {
        Dash,
        Fall,
        Hang,
        Hook,
        Movement,
        Pull,
        Push,
        Swing
    }

    public new Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;

    public ActionState currentActionState;
    public EntityState currentEntityState;

    public Dictionary<ActionState, EntityState> entityStates = new Dictionary<ActionState, EntityState>();

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitComponents();

        EntityState[] states = GetComponents<EntityState>();
        foreach (EntityState state in states)
        {
            entityStates.Add(state.GetOwnActionState(), state);
            state.InitComponents();
        }

        SetActionState(ActionState.Movement);
    }

    public abstract void InitComponents();

    public void SetActionState(ActionState newActionState)
    {
        currentEntityState = GetEntityState(newActionState);
        currentActionState = newActionState;
    }

    public abstract void Fall();

    public abstract void SetNormalLayer();
    public abstract void SetHigherLayer();

    public EntityState GetEntityState(ActionState actionState)
    {
        EntityState returnValue;
        entityStates.TryGetValue(actionState, out returnValue);
        return returnValue;
    }

    public void Push(float speed, Vector2 distanceVector)
    {
        PushState pushState = (PushState) GetEntityState(ActionState.Push);

        pushState.pushSpeed = speed;
        pushState.pushVector = distanceVector;
        pushState.pushTime = pushState.pushVector.magnitude / pushState.pushSpeed;
    }
}