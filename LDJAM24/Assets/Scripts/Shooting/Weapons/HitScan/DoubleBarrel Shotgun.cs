using UnityEngine;

public class DoubleBarrelShotgun : Gun
{
    public float fireRate = 1f; // Shots per second (slow fire rate)
    public float shotgunDamage = 10f; // High damage for close range
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

        // Set parameters for spread and recoil
        float bulletSpreadAngle = 20f; // Adjust this value for the spread
        float maxRecoilAngle = 10f; // Maximum recoil angle
        float minRecoilAngle = -10f; // Minimum recoil angle

        // Calculate the angle between each bullet in the spread
        float angleBetweenBullets = bulletSpreadAngle / (shotsCount - 1);

        // Calculate the starting angle for the spread along x-axis
        float startXAngle = -bulletSpreadAngle / 2f;

        // Calculate the starting angle for the spread along y-axis
        float startYAngle = -bulletSpreadAngle / 2f;

        for (int i = 0; i < shotsCount; i++)
        {
            // Calculate random recoil angles within the specified range for x and y axes
            float recoilXAngle = Random.Range(minRecoilAngle, maxRecoilAngle);
            float recoilYAngle = Random.Range(minRecoilAngle, maxRecoilAngle);

            // Calculate the rotation angles for this bullet (sum of spread and recoil for x and y axes)
            float rotationXAngle = startXAngle + (i * angleBetweenBullets) + recoilXAngle;
            float rotationYAngle = startYAngle + (i * angleBetweenBullets) + recoilYAngle;

            // Rotate the direction vector by the rotation angles
            Quaternion rotation = Quaternion.AngleAxis(rotationXAngle, Vector3.up) *
                                  Quaternion.AngleAxis(rotationYAngle, Vector3.right);
            Vector3 spreadDirection = rotation * direction;

            // Fire the bullet in the spread direction
            FireBullet(spreadDirection);
        }

        base.Shoot();
    }

    private void FireBullet(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(PlayerStats.instance.firepoint.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyStats target = hit.transform.GetComponent<EnemyStats>();

            if (target != null)
            {
                target.TakeDamage(shotgunDamage);
            }
        }

        CreatePhysicalBullet(direction, hit);
    }
}
