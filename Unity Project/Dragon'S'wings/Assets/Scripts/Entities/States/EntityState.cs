using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState : MonoBehaviour
{
    public Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();

        InitComponents();
    }
    
    public abstract void InitComponents();

    public virtual bool CheckInput()
    {
        return true;
    }

    public abstract void EnterState();

    public virtual void UpdateInput()
    {
        // Empty
    }

    public abstract void ExecuteAction();

    public abstract void ExitState();

    public abstract Entity.ActionState GetOwnActionState();
}