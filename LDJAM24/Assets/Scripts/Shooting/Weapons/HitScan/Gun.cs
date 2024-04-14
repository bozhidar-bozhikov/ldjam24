using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float damage = 10f;
    public float range = 100f;

    protected Camera fpsCam;

    public int maxBullets;
    public int bullets;

    private void Start()
    {
        fpsCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        bullets--;

        if (bullets <= 0) GunManager.RanOut();
    }

    protected GameObject CreatePhysicalBullet(Vector3 direction, RaycastHit hit)
    {
        GameObject bullet = CreatePhysicalBullet(direction);

        if (hit.collider != null)
        {
            bullet.transform.forward = hit.point - PlayerStats.instance.trailpoint.position;
            bullet.transform.localScale = new Vector3(1, 1, Vector3.Magnitude(PlayerStats.instance.firepoint.position - hit.point));
        }

        return bullet;
    }

    protected GameObject CreatePhysicalBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(ParameterManager.instance.playerBulletPrefab,
            PlayerStats.instance.trailpoint.position, Quaternion.identity);

        Destroy(bullet, ParameterManager.instance.playerBulletDestroyDelay);

        bullet.transform.forward = direction;

        bullet.transform.localScale = new Vector3(1, 1, 999);

        return bullet;
    }
}
