using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health;

    public float moveSpeed;
    public float attackDistance;
    public float attackDuration; //duration of attack
    public float attackCooldown; //time after attack before can attack again
    public float attackWindup; //time before the actual attack

    public Transform firepoint;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerStats.EnemyDied(GetComponent<AIStateMachine>().enemyType);
        Destroy(gameObject);
    }
}
