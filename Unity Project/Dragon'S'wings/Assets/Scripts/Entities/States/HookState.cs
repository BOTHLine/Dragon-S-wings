using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookState : EntityState
{
    public float maxRopeLength;

    public override void InitComponents()
    {
    //    throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
    //    throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
     //   throw new System.NotImplementedException();
    }

    public override void ExecuteAction()
    {
    //    throw new System.NotImplementedException();
    }

    public override bool CheckInput()
    {
        return (Input.GetButtonDown(InputList.Hook));
    }

    public override Entity.ActionState GetOwnActionState()
    {
        return Entity.ActionState.Hook;
    }
}