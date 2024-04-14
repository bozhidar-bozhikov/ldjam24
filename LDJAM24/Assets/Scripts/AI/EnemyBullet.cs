using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public EnemyBulletType type;

    private void OnTriggerEnter(Collider other)
    {
        if (type == EnemyBulletType.Ballistic && other.transform.CompareTag("Player"))
        {
            PlayerStats.TakeDamage();

            Destroy(gameObject);
        }
        else
        {
            RaycastHit[] hit = Physics.SphereCastAll(transform.position,
                ParameterManager.instance.enemyExplosiveRadius, transform.forward);

            foreach (RaycastHit item in hit)
            {
                if (item.transform.CompareTag("Player"))
                {
                    PlayerStats.TakeDamage();

                    Destroy(gameObject);

                    break;
                }
            }
        }
    }
}

public enum EnemyBulletType
{
    Ballistic,
    Explosive
}
