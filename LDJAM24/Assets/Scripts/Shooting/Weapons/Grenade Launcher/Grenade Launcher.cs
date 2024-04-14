using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Gun
{
    public GameObject grenadePrefab; // Prefab for the grenade object
    public float explosionRadius = 5f; // Radius of the damaging sphere zone
    public float explosionDamage = 100f; // Damage dealt within the sphere zone

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireGrenade();
        }
    }

    void FireGrenade()
    {
        // Instantiate the grenade prefab
        GameObject grenade = Instantiate(grenadePrefab, PlayerStats.instance.firepoint.position, Quaternion.identity);

        // Apply an initial force to the grenade
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        print(fpsCam.transform.forward);

        Vector3 cross = Vector3.Cross(fpsCam.transform.forward, fpsCam.transform.right).normalized 
            * ParameterManager.instance.playerGrenadeYAxisMultiplier;

        Vector3 direction = fpsCam.transform.forward + cross;

        rb.AddForce(direction * ParameterManager.instance.playerGrenadeForce, ForceMode.Impulse);

        base.Shoot();
    }
}
