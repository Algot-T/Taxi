using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Tilemap roadTilemap;
    public int maxEnemies = 20;
    public float minSpawnDistance = 10f;

    private List<Vector3> roadPositions = new List<Vector3>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Transform playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;

        CacheRoads();

        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }

        InvokeRepeating(nameof(SpawnEnemy), 3f, 3f);
    }

    void CacheRoads()
    {
        roadPositions.Clear();
        BoundsInt bounds = roadTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (roadTilemap.HasTile(pos))
            {
                roadPositions.Add(roadTilemap.GetCellCenterWorld(pos));
            }
        }
    }

    public void SpawnEnemy()
    {
        activeEnemies.RemoveAll(item => item == null);

        if (enemyPrefab == null || roadPositions.Count == 0 || activeEnemies.Count >= maxEnemies)
        {
            return;
        }

        Vector3 spawnPos = Vector3.zero;
        bool found = false;
        int attempts = 0;

        while (!found && attempts < 10)
        {
            spawnPos = roadPositions[Random.Range(0, roadPositions.Count)];

            if (playerTransform != null)
            {
                float dist = Vector3.Distance(spawnPos, playerTransform.position);
                if (dist >= minSpawnDistance)
                {
                    found = true;
                }
            }
            else
            {
                found = true;
            }
            attempts++;
        }

        if (found)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }
}