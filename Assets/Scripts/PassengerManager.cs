using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PassengerManager : MonoBehaviour
{
    public static PassengerManager Instance;

    public GameObject passengerPrefab;
    public Tilemap roadTilemap;

    [HideInInspector] public Transform currentPassenger;

    private List<Vector3> roadPositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CacheRoadPositions();
        SpawnPassenger();
    }

    void CacheRoadPositions()
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

    public void SpawnPassenger()
    {
        if (passengerPrefab == null) return;
        if (roadPositions.Count == 0) return;

        Vector3 spawnPos = roadPositions[Random.Range(0, roadPositions.Count)];

        GameObject passenger = Instantiate(passengerPrefab, spawnPos, Quaternion.identity);
        currentPassenger = passenger.transform;
        Debug.Log("SpawnPassenger called");
        Debug.Log("Road positions: " + roadPositions.Count);
    }
}