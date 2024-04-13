using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public static Transform player;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public int maxCells;
    public int cells;
    public int quota;
    public int currentQuota;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        quota = 0;
        cells = maxCells;
    }

    public static void ChangeCells(int amount)
    {
        instance.cells = Mathf.Clamp(instance.cells + amount, 0, instance.maxCells);

        if (instance.cells <= 0)
        {
            Debug.Log("player died");
        }
    }

    public static void TakeDamage()
    {
        ChangeCells(-1);
    }

    public static void EnemyDied(EnemyType type)
    {
        int quotaGained = 0;

        switch (type)
        {
            case EnemyType.Walker:
            case EnemyType.Trooper:
            case EnemyType.RocketTrooper: quotaGained = 1; break;
            case EnemyType.Berserker: quotaGained = 2; break;
            case EnemyType.Flamer: quotaGained = 3; break;
        }

        instance.currentQuota += quotaGained;
        if (instance.currentQuota >= instance.quota)
        {
            instance.currentQuota -= instance.quota;
            ChangeCells(1);
        }
    }
}
