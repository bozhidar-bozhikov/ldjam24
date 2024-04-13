using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State
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

public class TrooperMove : MoveState
{
    public override void OnEnter(AIStateMachine sm)
    {

    }

    public override void OnUpdate(AIStateMachine sm)
    {
        NavMeshAgent navMeshAgent = sm.agent.navMesh;
        Transform player = sm.agent.player;

        navMeshAgent.SetDestination(player.position);

        if (Vector3.Distance(sm.transform.position, player.position) < sm.agent.stats.attackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
    }

    public override void OnExit(AIStateMachine sm)
    {
        sm.agent.navMesh.SetDestination(sm.transform.position);
    }
}

public class StationaryMove : MoveState
{
    public override void OnEnter(AIStateMachine sm)
    {
        
    }

    public override void OnUpdate(AIStateMachine sm)
    {
        Transform player = sm.agent.player;

        if (Vector3.Distance(sm.transform.position, player.position) < sm.agent.stats.attackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
    }

    public override void OnExit(AIStateMachine sm)
    {
        
    }
}

public class FlamerMove : MoveState //flamer can shoot while moving, but stops while flaming
{
    bool canShoot;

    public override void OnEnter(AIStateMachine sm)
    {
        canShoot = true;
    }

    public override void OnUpdate(AIStateMachine sm) //flamer shoots when < attackdistance and walks
    {
        NavMeshAgent navMeshAgent = sm.agent.navMesh;
        Transform player = sm.agent.player;

        navMeshAgent.SetDestination(player.position);

        if (Vector3.Distance(sm.transform.position, player.position) < ParameterManager.instance.flamerAttackDistance)
        {
            sm.ChangeState(sm.attackState);
        }
        else
        {
            if (canShoot && Vector3.Distance(sm.transform.position, player.position) < sm.stats.attackDistance)
            {
                canShoot = false;
                sm.StartCoroutine(Shoot(sm));
            }
        }
    }

    public override void OnExit(AIStateMachine sm)
    {
        sm.agent.navMesh.SetDestination(sm.transform.position);
    }

    private IEnumerator Shoot(AIStateMachine sm)
    {
        GameObject bullet = ParameterManager.CreateGameObject(ParameterManager.instance.bulletPrefab);
        bullet.transform.parent = sm.stats.firepoint;
        bullet.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(sm.stats.attackWindup);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = sm.agent.player.position - sm.stats.firepoint.position;
        bullet.GetComponent<EnemyBullet>().type =
            (sm.enemyType == EnemyType.Trooper) ? EnemyBulletType.Ballistic : EnemyBulletType.Explosive;

        rb.AddForce(direction * ParameterManager.instance.enemyBulletForce, ForceMode.Impulse);

        yield return new WaitForSeconds(sm.stats.attackDuration);

        yield return new WaitForSeconds(sm.stats.attackCooldown);

        sm.ChangeState(sm.idleState);

        canShoot = true;
    }
}