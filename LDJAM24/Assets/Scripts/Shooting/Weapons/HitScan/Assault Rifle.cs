using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    public float fireRate = 3f;
    public float ARDamage = 6f;
    private float nextTimeToFire = 0f;

    protected override void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(PlayerStats.instance.firepoint.position, PlayerStats.instance.firepoint.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(ARDamage);
            }
        }

        CreatePhysicalBullet(fpsCam.transform.forward, hit);
        base.Shoot();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
}
