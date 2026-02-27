using UnityEngine;

public class IndicatorArrow : MonoBehaviour
{
    [Header("References")]
    public Transform player;                 
    public PlayerController playerController; 

    [Header("Settings")]
    public float radius = 1.5f;              
    public float smoothSpeed = 5f;           

    private Vector2 currentPos;

    void Start()
    {
        if (player != null)
            currentPos = (Vector2)player.position + Vector2.up * radius;
    }

    void Update()
    {
        if (player == null || playerController == null) return;

        Transform target = null;

        if (playerController.HasPassenger())
            target = DestinationManager.Instance?.currentDestination;
        else
            target = PassengerManager.Instance?.currentPassenger;

        if (target == null) return;

        Vector2 dir = ((Vector2)target.position - (Vector2)player.position).normalized;
        Vector2 desiredPos = (Vector2)player.position + dir * radius;

        currentPos = Vector2.Lerp(currentPos, desiredPos, Time.deltaTime * smoothSpeed);

        transform.position = currentPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}