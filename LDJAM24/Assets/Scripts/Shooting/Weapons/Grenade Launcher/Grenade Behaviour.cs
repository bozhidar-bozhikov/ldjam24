using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    public float explosionRadius = 5f; // Radius of the damaging sphere zone
    public float explosionDamage = 100f; // Damage dealt within the sphere zone


    // Called when the grenade hits the ground or an obstacle
    void OnCollisionEnter(Collision collision)
    {
        // Create a sphere zone at the impact point
        Collider[] colliders = Physics.OverlapSphere(collision.contacts[0].point, explosionRadius);

        foreach (Collider hitCollider in colliders)
        {
            // Check if the collider has a Target component (you can adjust this based on your game)
            Target target = hitCollider.GetComponent<Target>();
            if (target != null)
            {
                // Apply damage to targets within the sphere zone
                target.TakeDamage(explosionDamage);
            }
        }

        // Destroy the grenade
        Destroy(gameObject);
    }
}
