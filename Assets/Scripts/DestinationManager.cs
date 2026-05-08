using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;

    public Tilemap buildingTilemap;
    public GameObject deliveryZonePrefab;
    private GameObject activeDeliveryZone;

    private List<Vector3> buildingPositions = new List<Vector3>();

    // Denna rad fixar felet i IndicatorArrow.cs
    public Vector3 currentDestination => activeDeliveryZone != null ? activeDeliveryZone.transform.position : PlayerController.Instance.transform.position;

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

        if (activeDeliveryZone != null) Destroy(activeDeliveryZone);

        Vector3 randomPos = buildingPositions[Random.Range(0, buildingPositions.Count)];
        activeDeliveryZone = Instantiate(deliveryZonePrefab, randomPos, Quaternion.identity);
    }

    public void ClearDestination()
    {
        if (activeDeliveryZone != null) Destroy(activeDeliveryZone);
    }
}