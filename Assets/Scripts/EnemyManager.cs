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

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) return;

        Vector2 spawnPos = MapManager.Instance.GetRandomFreePosition(Vector2.zero, 0f);

        if (Vector2.Distance(spawnPos, playerGO.transform.position) >= 7f)
        {
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}