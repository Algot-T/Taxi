using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public int width = 20;
    public int height = 20;

    public GameObject workshopPrefab;

    [HideInInspector]
    public List<Vector2> freePositions = new List<Vector2>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GenerateMap();
        SpawnWorkshop();
    }

    void GenerateMap()
    {
        freePositions.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                freePositions.Add(pos);
            }
        }
    }

    void SpawnWorkshop()
    {
        Vector2 center = GetCenter();
        Vector2 workshopPos = center + new Vector2(3f, 0f);
        Instantiate(workshopPrefab, workshopPos, Quaternion.identity);
    }

    public Vector2 GetRandomFreePosition(Vector2 avoidPosition, float minDistance = 3f)
    {
        List<Vector2> candidates = freePositions.FindAll(p => Vector2.Distance(p, avoidPosition) >= minDistance);

        if (candidates.Count == 0) return Vector2.zero;

        return candidates[Random.Range(0, candidates.Count)];
    }

    public Vector2 GetCenter()
    {
        return new Vector2(width / 2f, height / 2f);
    }

    public int Width => width;
    public int Height => height;
}