using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Tilemap roadTilemap;

    private List<Vector3> roadPositions = new List<Vector3>();

    private void Start()
    {
        CacheRoads();
        InvokeRepeating(nameof(SpawnEnemy), 1f, 3f);
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
        if (enemyPrefab == null) return;
        if (roadPositions.Count == 0) return;

        Vector3 spawnPos = roadPositions[Random.Range(0, roadPositions.Count)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}