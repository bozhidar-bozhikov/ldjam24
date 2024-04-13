using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Gun
{
    public float piercingDamage = 25f; // High damage value for piercing shots
    public float piercingRange = 150f; // Increased range for piercing shots

    protected override void Shoot()
    {
        RaycastHit[] hits = Physics.RaycastAll(fpsCam.transform.position, fpsCam.transform.forward, piercingRange);

        foreach(RaycastHit hit in hits)
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(piercingDamage);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
}
