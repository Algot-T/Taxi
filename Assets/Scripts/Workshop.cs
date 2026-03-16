using UnityEngine;

public class Workshop : MonoBehaviour
{
    public static Workshop Instance;

    [Header("UI")]
    public GameObject workshopUI;

    [Header("Spawn Settings")]
    public float minDistanceFromPlayer = 5f;
    public float maxDistanceFromPlayer = 10f;

    [HideInInspector]
    public Transform currentDestination;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentDestination = transform;
    }

    void Start()
    {
        SpawnNearPlayer();
    }

    void SpawnNearPlayer()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) return;

        Vector2 spawnPos = Vector2.zero;
        int attempts = 0;

        while (attempts < 100)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
            spawnPos = (Vector2)playerGO.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

            if (Vector2.Distance(spawnPos, playerGO.transform.position) >= minDistanceFromPlayer)
                break;

            attempts++;
        }

        transform.position = spawnPos;
        currentDestination = transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (workshopUI != null)
            {
                workshopUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (workshopUI != null)
            {
                workshopUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}