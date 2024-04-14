using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIStateMachine))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMesh;
    [HideInInspector] public Transform player;
    [HideInInspector] public EnemyStats stats;
    [HideInInspector] public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();

        navMesh.speed = stats.moveSpeed;
    }
}
