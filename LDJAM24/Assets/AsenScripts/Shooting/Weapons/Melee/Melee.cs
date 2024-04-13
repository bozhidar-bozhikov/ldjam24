using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRadius = 2f; // Radius of the sphere cast
    public float attackDamage = 30f; // Damage dealt to targets within the sphere
    public float attackCooldown = 1.5f; // Cooldown time between attacks
    public Transform firePoint; // Point where the melee attack is performed

    private bool canAttack = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            PerformMeleeAttack();
        }
    }

    void PerformMeleeAttack()
    {
        // Cast a sphere in front of the player
        Collider[] colliders = Physics.OverlapSphere(firePoint.transform.position, attackRadius);

        foreach (Collider hitCollider in colliders)
        {
            // Check if the collider has a Target component (you can adjust this based on your game)
            Target target = hitCollider.GetComponent<Target>();
            if (target != null)
            {
                // Apply damage to targets within the sphere
                target.TakeDamage(attackDamage);
                Debug.Log("Hit " + target.name);
            }
        }

        // Start the attack cooldown
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
