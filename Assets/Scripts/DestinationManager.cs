using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;

    [Header("Prefab & Spawn Area")]
    public GameObject destinationPrefab;
    public Vector2 spawnAreaMin = new Vector2(-8, -4);
    public Vector2 spawnAreaMax = new Vector2(8, 4);

    [HideInInspector]
    public Transform currentDestination;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnDestination()
    {
        if (destinationPrefab == null)
        {
            Debug.LogWarning("Destination prefab saknas!");
            return;
        }

        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) return;

        Vector2 spawnPos = MapManager.Instance.GetRandomFreePosition(
            playerGO.transform.position, 5f);

        GameObject dest = Instantiate(destinationPrefab, spawnPos, Quaternion.identity);
        dest.SetActive(true);
        currentDestination = dest.transform;

        Debug.Log("Destination spawned at: " + spawnPos);
    }
}