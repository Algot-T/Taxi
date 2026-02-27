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

        Vector2 playerPos = playerGO.transform.position;

        float minDistance = 3f;
        float maxDistance = 8f;

        Vector2 spawnPos = Vector2.zero;
        bool found = false;

        int attempts = 0;

        while (!found && attempts < 40)
        {
            // slumpa riktning
            Vector2 dir = Random.insideUnitCircle.normalized;

            // slumpa avstånd inom intervallet
            float dist = Random.Range(minDistance, maxDistance);

            Vector2 candidate = playerPos + dir * dist;

            // checka att platsen är fri
            Collider2D hit = Physics2D.OverlapCircle(candidate, 0.4f);

            if (hit == null) // inget blockerar platsen
            {
                spawnPos = candidate;
                found = true;
            }

            attempts++;
        }

        if (!found)
        {
            Debug.LogWarning("Kunde inte hitta spawnplats nära spelaren.");
            return;
        }

        GameObject passenger = Instantiate(passengerPrefab, spawnPos, Quaternion.identity);
        currentPassenger = passenger.transform;

        Debug.Log("Passenger spawned near player at: " + spawnPos);
    }
}