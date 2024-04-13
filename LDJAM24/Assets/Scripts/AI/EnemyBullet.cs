using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public EnemyBulletType type;

    private void OnCollisionEnter(Collision collision)
    {
        if (type == EnemyBulletType.Ballistic && collision.transform.CompareTag("Player"))
        {
            PlayerStats.TakeDamage();
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
                    break;
                }
            }
        }

        Destroy(gameObject);
    }
}

public enum EnemyBulletType
{
    Ballistic,
    Explosive
}
