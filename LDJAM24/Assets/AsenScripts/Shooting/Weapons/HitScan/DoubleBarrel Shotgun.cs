using UnityEngine;

public class DoubleBarrelShotgun : Gun
{
    public float fireRate = 1f; // Shots per second (slow fire rate)
    public float shotgunDamage = 20f; // High damage for close range
    public float bulletSpreadAngle = 5f; // Angle in degrees for bullet spread

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    protected override void Shoot()
    {
        Vector3 mainBulletDirection = fpsCam.transform.forward;
        Vector3 secondBulletDirection = Quaternion.Euler(0f, bulletSpreadAngle, 0f) * mainBulletDirection;

        FireBullet(mainBulletDirection);
        FireBullet(secondBulletDirection);
    }

    private void FireBullet(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(shotgunDamage);
            }
        }
    }
}
