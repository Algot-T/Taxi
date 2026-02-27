using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    public static PassengerManager Instance;

    [Header("Prefab & Spawn Area")]
    public GameObject passengerPrefab;
    public Vector2 spawnAreaMin = new Vector2(-8, -4);
    public Vector2 spawnAreaMax = new Vector2(8, 4);

    [HideInInspector]
    public Transform currentPassenger;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SpawnPassenger();
    }

    public void SpawnPassenger()
    {
        if (passengerPrefab == null)
        {
            Debug.LogWarning("Passenger prefab saknas!");
            return;
        }

        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) return;

        Vector2 spawnPos = MapManager.Instance.GetRandomFreePosition(
            playerGO.transform.position, 5f);

        GameObject passenger = Instantiate(passengerPrefab, spawnPos, Quaternion.identity);
        passenger.SetActive(true);
        currentPassenger = passenger.transform;

        Debug.Log("Passenger spawned at: " + spawnPos);
    }
}