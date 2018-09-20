using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Entity : MonoBehaviour
{
    public enum ActionState
    {
        None = (1 << 0),
        Dash = (1 << 1),
        Fall = (1 << 2),
        Hang = (1 << 3),
        Hook = (1 << 4),
        Movement = (1 << 5),
        Pull = (1 << 6),
        Push = (1 << 7),
        Swing = (1 << 8),
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

        SetActionState(new MovementStateParameter());
    }

    public abstract void InitOwnComponents();
    public abstract void InitOtherComponents();

    public void SetActionState(EntityStateParameter entityStateParameter)
    {
        if (currentEntityState != null)
        {
            currentEntityState.ExitState();
        }
        ActionState newActionState = entityStateParameter.GetActionState();
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