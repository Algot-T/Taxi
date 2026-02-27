using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public GameObject enemyPrefab;
    public int maxEnemies = 5;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) return;

        int attempts = 0;
        int spawned = 0;

        while (spawned < maxEnemies && attempts < 100) // max 100 försök för att undvika oändlig loop
        {
            Vector2 spawnPos = MapManager.Instance.GetRandomFreePosition(Vector2.zero, 0f);

            // Avståndskontroll: minst 7 rutor från spelaren
            if (Vector2.Distance(spawnPos, playerGO.transform.position) >= 7f)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawned++;
            }

            attempts++;
        }

        Debug.Log("Fiender spawnade: " + spawned);
    }
}