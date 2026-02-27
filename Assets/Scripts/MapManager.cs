// MapManager.cs (grund)
using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public int width = 20;
    public int height = 20;
    public GameObject obstaclePrefab;
    [Range(0f, 1f)]
    public float obstacleChance = 0.2f;
    [HideInInspector]
    public List<Vector2> freePositions = new List<Vector2>();

    void Awake() { Instance = this; }

    void Start() { GenerateMap(); }

    void GenerateMap()
    {
        freePositions.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (Random.value < obstacleChance)
                    Instantiate(obstaclePrefab, pos, Quaternion.identity);
                else
                    freePositions.Add(pos);
            }
        }
    }

    public Vector2 GetRandomFreePosition(Vector2 avoidPosition, float minDistance = 3f)
    {
        List<Vector2> candidates = freePositions.FindAll(p => Vector2.Distance(p, avoidPosition) >= minDistance);
        return candidates.Count == 0 ? Vector2.zero : candidates[Random.Range(0, candidates.Count)];
    }
}