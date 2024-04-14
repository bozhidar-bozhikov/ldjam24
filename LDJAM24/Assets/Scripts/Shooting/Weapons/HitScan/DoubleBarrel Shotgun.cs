using UnityEngine;

public class DoubleBarrelShotgun : Gun
{
    public float fireRate = 1f; // Shots per second (slow fire rate)
    public float shotgunDamage = 10f; // High damage for close range
    public float bulletSpreadAngle = 5f; // Angle in degrees for bullet spread
    public float concentrationFactor = 0.5f;
    public int shotsCount = 5;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    protected override void Shoot() // TODO: redo
    {
        Vector3 direction = fpsCam.transform.forward;

        for (int i = 0; i < shotsCount; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere;

            // Interpolate between random direction and target direction based on concentration factor
            Vector3 concentratedDirection = Vector3.Lerp(randomDirection, direction, concentrationFactor);

            // Calculate rotation angle based on the spread angle
            float rotationAngle = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);

            // Rotate the concentrated direction by the rotation angle around the target direction
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, direction);
            Vector3 finalDirection = rotation * concentratedDirection;

            FireBullet(finalDirection);
        }

        base.Shoot();
    }

    private void FireBullet(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(PlayerStats.instance.firepoint.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(shotgunDamage);
            }
        }

        CreatePhysicalBullet(direction, hit);
    }
}
