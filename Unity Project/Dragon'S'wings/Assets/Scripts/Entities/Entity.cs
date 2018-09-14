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
    public CircleCollider2D circleCollider2D;
    public SpriteRenderer spriteRenderer;

    public ActionState currentActionState;
    public EntityState currentEntityState;

    public Dictionary<ActionState, EntityState> entityStates = new Dictionary<ActionState, EntityState>();

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitOwnComponents();

        EntityState[] states = GetComponents<EntityState>();
        foreach (EntityState state in states)
        {
            entityStates.Add(state.GetOwnActionState(), state);
        }
    }

    private void Start()
    {
        InitOtherComponents();

        SetActionState(ActionState.Movement, new MovementStateParameter());
    }

    public abstract void InitOwnComponents();
    public abstract void InitOtherComponents();

    public void SetActionState(ActionState newActionState, EntityStateParameter entityStateParameter)
    {
        if (currentEntityState)
        {
            currentEntityState.ExitState();
        }
        currentEntityState = GetEntityState(newActionState);
        currentActionState = newActionState;
        currentEntityState.EnterState(entityStateParameter);
    }

    public abstract void Fall(); // TODO: Handle new State!!

    public abstract void SetNormalLayer();
    public abstract void SetHigherLayer();

    public EntityState GetEntityState(ActionState actionState)
    {
        EntityState returnValue;
        entityStates.TryGetValue(actionState, out returnValue);
        return returnValue;
    }

    public abstract Vector2 GetAimingVector();
}