using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    public EnemyType enemyType;

    State currentState;
    public MoveState moveState = new MoveState();
    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();

    [HideInInspector] public Agent agent;
    [HideInInspector] public EnemyStats stats;

    private void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
        stats = gameObject.GetComponent<EnemyStats>();
    }

    private void Start()
    {
        switch (enemyType)
        {
            case EnemyType.Walker:
            case EnemyType.Berserker:
                {
                    moveState = new TrooperMove();
                    idleState = new DefaultIdle();
                    attackState = new WalkerAttack();
                } break;
            case EnemyType.Trooper:
            case EnemyType.RocketTrooper:
                {
                    moveState = new TrooperMove();
                    idleState = new DefaultIdle();
                    attackState = new TrooperAttack();
                } break;
            case EnemyType.Flamer:
                {
                    moveState = new FlamerMove();
                    idleState = new DefaultIdle();
                    attackState = new FlamerAttack();
                } break;
            default:
                break;
        }

        currentState = moveState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(State newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}

public abstract class State
{
    public abstract void EnterState(AIStateMachine sm);
    public abstract void UpdateState(AIStateMachine sm);
    public abstract void ExitState(AIStateMachine sm);
}

public enum EnemyType
{
    Walker,
    Berserker,
    Trooper,
    RocketTrooper,
    Flamer
}
