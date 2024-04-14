using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterManager : MonoBehaviour
{
    public static ParameterManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public GameObject bulletPrefab;
    public GameObject playerBulletPrefab;

    public float playerBulletDestroyDelay;
    public float playerGrenadeForce;
    public float playerGrenadeYAxisMultiplier;
    public float enemyBulletForce;
    public float enemyExplosiveRadius;
    public float flamerAttackDistance;
    public float flamerFlameLength;
    public float flamerFlameRadius;
    public float flamerDamageTickSpeed; //in seconds
    public float flamerFlameCooldown;

    public static GameObject CreateGameObject(GameObject obj)
    {
        GameObject inst = Instantiate(obj);
        return inst;
    }
}
