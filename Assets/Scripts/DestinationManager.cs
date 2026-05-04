using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;

    public Tilemap buildingTilemap;

    [HideInInspector] public Vector3 currentDestination;

    private List<Vector3> buildingPositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CacheBuildingPositions();
    }

    void CacheBuildingPositions()
    {
        buildingPositions.Clear();

        BoundsInt bounds = buildingTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (buildingTilemap.HasTile(pos))
            {
                buildingPositions.Add(buildingTilemap.GetCellCenterWorld(pos));
            }
        }
    }

    public void SetRandomDestination()
    {
        if (buildingPositions.Count == 0) return;

        currentDestination = buildingPositions[Random.Range(0, buildingPositions.Count)];
    }
}