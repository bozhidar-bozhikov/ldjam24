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
        FindObjectOfType<SoundManager>().Play("Enemy_Hurt");

        health -= damage;

        if (health <= 0)
        {
            Die();
        }

        Debug.Log("Enemy took damage. Add Particles");
    }

    private void Die()
    {
        FindObjectOfType<SoundManager>().Play("Enemy_Die");

        PlayerStats.EnemyDied(GetComponent<AIStateMachine>().enemyType);
        
        // Reset the timer and add score with the current multiplier
        GameManager.instance.EnemyKilled();

        GameManager.instance.AddScoreWithMultiplier(10);

        Debug.Log("Enemy died");
        Debug.Log("Score: " + GameManager.instance.score);
        Destroy(gameObject);
    }
}
