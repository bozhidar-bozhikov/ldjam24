using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : Gun
{
    public float fireRate = 10f;
    public float smgDamage = 2f;
    private float nextTimeToFire = 0f;


    protected override void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(smgDamage);
            }

        }

    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
}
