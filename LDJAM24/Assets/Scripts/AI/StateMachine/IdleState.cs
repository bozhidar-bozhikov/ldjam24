using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public override void EnterState(AIStateMachine sm)
    {
        OnEnter(sm);
    }

    public virtual void OnEnter(AIStateMachine sm)
    {
        // to be overridden
    }

    public override void UpdateState(AIStateMachine sm)
    {
        OnUpdate(sm);
    }

    public virtual void OnUpdate(AIStateMachine sm)
    {
        //to be overriden
    }

    public override void ExitState(AIStateMachine sm)
    {
        OnExit(sm);
    }

    public virtual void OnExit(AIStateMachine sm)
    {
        //to be overriden
    }
}

public class DefaultIdle : IdleState
{
    public override void OnEnter(AIStateMachine sm)
    {
        sm.agent.animator.SetTrigger("Idle");
    }

    public override void OnUpdate(AIStateMachine sm)
    {
        Transform player = sm.agent.player;

        if (Vector3.Distance(sm.transform.position, player.position) > sm.agent.stats.attackDistance)
        {
            sm.ChangeState(sm.moveState);
        }
        else
        {
            sm.ChangeState(sm.attackState);
        }
    }

    public override void OnExit(AIStateMachine sm)
    {
        sm.agent.animator.ResetTrigger("Idle");
    }
}



