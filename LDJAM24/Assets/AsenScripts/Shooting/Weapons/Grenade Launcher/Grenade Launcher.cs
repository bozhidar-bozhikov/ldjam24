using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Gun
{
    public GameObject grenadePrefab; // Prefab for the grenade object
    public float launchForce = 20f; // Initial launch force
    public float explosionRadius = 5f; // Radius of the damaging sphere zone
    public float explosionDamage = 100f; // Damage dealt within the sphere zone
    public Transform firePoint; // Point where the grenade is launched from

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireGrenade();
        }
    }

    void FireGrenade()
    {
        // Instantiate the grenade prefab
        GameObject grenade = Instantiate(grenadePrefab, firePoint.transform.position, Quaternion.identity);

        // Apply an initial force to the grenade
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(fpsCam.transform.forward * launchForce, ForceMode.Impulse);
    }
}
