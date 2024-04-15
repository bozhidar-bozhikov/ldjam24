using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRadius = 2f; // Radius of the sphere cast
    public float attackDamage = 30f; // Damage dealt to targets within the sphere
    public float attackCooldown = 1.5f; // Cooldown time between attacks
    public Transform firePoint; // Point where the melee attack is performed

    [SerializeField]
    private bool canAttack = true;
    [SerializeField]
    private bool isEnabled = false; 
    public Animator animator;
    public float attackAnimDelay = 1.25f;
    public float attackCastTime;
    public int attackCasts;
    public float unsheatheTime;
    public float sheatheTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && isEnabled)
        {
            PerformMeleeAttack();
        }
    }

    public IEnumerator Unsheathe()
    {
        animator.SetBool("Show", true);
        yield return new WaitForSeconds(unsheatheTime);
        isEnabled = true;
    }

    public IEnumerator Sheathe()
    {
        animator.SetBool("Show", false);
        isEnabled = false;
        yield return new WaitForSeconds(sheatheTime);
        //smth
    }

    public void PerformMeleeAttack()
    {
        StartCoroutine(_PerformMeleeAttack());
    }

    bool at;

    private IEnumerator _PerformMeleeAttack()
    {
        canAttack = false;
        float cooldown;

        if (Random.Range(0, 2) == 0)
        {
            cooldown = 2.25f;
            animator.SetBool("Strike", true);
        }
        else
        {
            cooldown = 3.042f;
            animator.SetBool("Overhead", true);
        }
            
        yield return new WaitForSeconds(attackAnimDelay);

        List<Collider> cols = new List<Collider>();

        at = true;

        for (int i = 0; i < attackCasts; i++)
        {
            Collider[] colliders = Physics.OverlapSphere(firePoint.transform.position, attackRadius);

            foreach (Collider col in colliders)
            {
                if (!cols.Contains(col))
                {
                    cols.Add(col);

                    EnemyStats target = col.transform.GetComponent<EnemyStats>();
                    if (target != null)
                    {
                        // Apply damage to targets within the sphere
                        target.TakeDamage(attackDamage);
                        Debug.Log("Hit " + target.name);
                    }
                }
            }

            yield return new WaitForSeconds(attackCastTime);
        }

        at = false;

        animator.SetBool("Strike", false);
        animator.SetBool("Overhead", false);

        yield return new WaitForSeconds(cooldown - attackAnimDelay - attackCastTime * attackCasts);

        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (at)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(firePoint.position, attackRadius);
        }
    }
}
