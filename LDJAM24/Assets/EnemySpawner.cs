using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector3 topRight;
    public Vector3 bottomLeft;
    public int separation;

    public Vector3[,] spawnPositions;
    public float playerClearRadius;

    // Start is called before the first frame update
    void Start()
    {
        GenerateSpawnPositions();
    }

    private void GenerateSpawnPositions()
    {
        float width = Mathf.Abs(topRight.x - bottomLeft.x);
        float height =Mathf.Abs(topRight.z - bottomLeft.z);

        int x = Mathf.RoundToInt(width) / separation;
        int y = Mathf.RoundToInt(height) / separation;

        spawnPositions = new Vector3[x,y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float posX = bottomLeft.x + i * separation;
                float posZ = bottomLeft.z + j * separation;

                RaycastHit hit;

                if (Physics.Raycast(new Vector3(posX, 100, posZ), Vector3.down, out hit))
                {
                    spawnPositions[i, j] = hit.point;
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
            Gizmos.DrawWireSphere(item, 0.15f);
        }

    }
}
