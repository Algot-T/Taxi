using UnityEngine;

public class Workshop : MonoBehaviour
{
    public static Workshop Instance;

    [Header("UI")]
    public GameObject workshopUI;

    [Header("Spawn Settings")]
    public float minDistanceFromPlayer = 5f;
    public float maxDistanceFromPlayer = 10f;

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