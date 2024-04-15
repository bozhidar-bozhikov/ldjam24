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

        if (Physics.Raycast(PlayerStats.instance.firepoint.position, PlayerStats.instance.firepoint.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyStats target = hit.transform.GetComponent<EnemyStats>();

            if (target != null)
            {
                target.TakeDamage(smgDamage);
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
