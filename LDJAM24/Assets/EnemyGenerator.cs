using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public Vector3 topRight;
    public Vector3 bottomLeft;
    public int separation;

    public Vector3[,] spawnPositions;
    public float playerClearRadius;

    public GameObject[] enemyPrefabs;
    public float spawnCooldown;
    public float cooldownReduction;
    public float minCooldown;

    // Start is called before the first frame update
    void Start()
    {
        GenerateSpawnPositions();

        StartCoroutine(SpawnWithDelay());
    }

    public void SpawnEnemy()
    {
        Vector3 node = new Vector3();

        bool flag = false;
        while (!flag)
        {
            Vector3 randomPos = spawnPositions[Random.Range(0, spawnPositions.GetLength(0)),
            Random.Range(0, spawnPositions.GetLength(1))];

            if (randomPos == new Vector3(999, 999, 999)) continue;

            Vector3 playerPos = new Vector3(PlayerStats.player.position.x, 0, PlayerStats.player.position.z);
            Vector3 normedPos = new Vector3(randomPos.x, 0, randomPos.z);

            if (Vector3.Distance(playerPos, normedPos) > playerClearRadius)
            {
                node = randomPos;
                flag = true;
            }
        }

        GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], node, Quaternion.identity);
    }

    IEnumerator SpawnWithDelay()
    {
        SpawnEnemy();

        yield return new WaitForSeconds(spawnCooldown);

        spawnCooldown = Mathf.Clamp(spawnCooldown - cooldownReduction, minCooldown, 999);
        StartCoroutine(SpawnWithDelay());
    }

    private void GenerateSpawnPositions()
    {
        topRight += new Vector3(1, 0, 1);

        float width = Mathf.Abs(topRight.x - bottomLeft.x);
        float height = Mathf.Abs(topRight.z - bottomLeft.z);

        int x = Mathf.RoundToInt(width) / separation;
        int y = Mathf.RoundToInt(height) / separation;

        spawnPositions = new Vector3[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float posX = bottomLeft.x + i * separation;
                float posZ = bottomLeft.z + j * separation;

                RaycastHit hit;

                if (Physics.Raycast(new Vector3(posX, 100, posZ), Vector3.down, out hit))
                {
                    if (hit.transform.CompareTag("Ground"))
                        spawnPositions[i, j] = hit.point;
                    else
                        spawnPositions[i, j] = new Vector3(999, 999, 999);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(topRight, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottomLeft, 1);

        if (spawnPositions == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(PlayerStats.player.position, playerClearRadius);

        Gizmos.color = Color.gray;
        foreach (Vector3 item in spawnPositions)
        {
            if (item != new Vector3(999, 999, 999)) 
                Gizmos.DrawWireSphere(item, 0.15f);
        }

    }
}
