using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
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

        //TODO improve
        sm.transform.LookAt(PlayerStats.player, Vector3.up);
        sm.transform.eulerAngles = new Vector3(0, sm.transform.eulerAngles.y, 0);
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

public class FlamerAttack : AttackState
{
    public override void OnEnter(AIStateMachine sm)
    {
        sm.StartCoroutine(Flamethrower(sm));
    }

    private IEnumerator Flamethrower(AIStateMachine sm)
    {
        yield return new WaitForSeconds(sm.stats.attackWindup);

        Vector3 origin = sm.stats.firepoint.position;
        Vector3 direction = sm.agent.player.position - sm.stats.firepoint.position;
        Vector3 final = direction.normalized * ParameterManager.instance.flamerFlameLength + origin;

        float timer = 0;
        
        while (timer < sm.stats.attackDuration)
        {
            Collider[] colliders = Physics.OverlapCapsule(origin, final, ParameterManager.instance.flamerFlameRadius);
            foreach (Collider col in colliders)
            {
                if (col.transform.CompareTag("Player"))
                {
                    PlayerStats.TakeDamage();
                    break;
                }
            }

            timer += ParameterManager.instance.flamerDamageTickSpeed;
            yield return new WaitForSeconds(ParameterManager.instance.flamerDamageTickSpeed);
        }

        yield return new WaitForSeconds(ParameterManager.instance.flamerFlameCooldown);

        sm.ChangeState(sm.idleState);
    }

    public override void OnUpdate(AIStateMachine sm)
    {

    }

    public override void OnExit(AIStateMachine sm)
    {

    }
}


public class TrooperAttack : AttackState
{
    public override void OnEnter(AIStateMachine sm)
    {
        //check line of sight
        Vector3 direction = PlayerStats.player.position - sm.stats.firepoint.position;
        RaycastHit hit;
        if (Physics.Raycast(sm.stats.firepoint.position, direction, out hit))
        {
            if (hit.transform.CompareTag("Player")) sm.StartCoroutine(Shoot(sm));
            else sm.ChangeState(sm.idleState);
        }
    }

    private IEnumerator Shoot(AIStateMachine sm)
    {
        GameObject bullet = ParameterManager.CreateGameObject(ParameterManager.instance.bulletPrefab);
        bullet.transform.parent = sm.stats.firepoint;
        bullet.transform.position = sm.stats.firepoint.position;

        yield return new WaitForSeconds(sm.stats.attackWindup);

        if (bullet != null)
        {
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Vector3 direction = sm.agent.player.position - sm.stats.firepoint.position;
            bullet.GetComponent<EnemyBullet>().type =
                (sm.enemyType == EnemyType.Trooper) ? EnemyBulletType.Ballistic : EnemyBulletType.Explosive;

            rb.AddForce(direction * ParameterManager.instance.enemyBulletForce, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(sm.stats.attackDuration);

        yield return new WaitForSeconds(sm.stats.attackCooldown);

        sm.ChangeState(sm.idleState);
    }

    public override void OnUpdate(AIStateMachine sm)
    {

    }

    public override void OnExit(AIStateMachine sm)
    {

    }
}

public class WalkerAttack : AttackState
{
    public override void OnEnter(AIStateMachine sm)
    {
        sm.agent.animator.SetTrigger("Attack");

        sm.StartCoroutine(Slash(sm));
    }

    private IEnumerator Slash(AIStateMachine sm)
    {
        yield return new WaitForSeconds(sm.stats.attackWindup);

        if (Random.Range(1, 3) == 1) sm.agent.animator.SetTrigger("Overhead");
        else sm.agent.animator.SetTrigger("Shank");

        if (Vector3.Distance(sm.agent.player.position, sm.transform.position) < sm.stats.attackDistance)
        {
            PlayerStats.TakeDamage();
        }
        yield return new WaitForSeconds(sm.stats.attackDuration);

        yield return new WaitForSeconds(sm.stats.attackCooldown);

        sm.ChangeState(sm.idleState);
    }

    public override void OnUpdate(AIStateMachine sm)
    {
        
    }

    public override void OnExit(AIStateMachine sm)
    {

    }
}

