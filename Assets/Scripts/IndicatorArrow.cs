using UnityEngine;

public class IndicatorArrow : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerController playerController;

    [Header("Settings")]
    public float radius = 1.5f;
    public float smoothSpeed = 5f;

    [Header("Colors")]
    public Color passengerColor = Color.blue;
    public Color destinationColor = Color.green;
    public Color workshopColor = Color.yellow;

    public enum ArrowTarget
    {
        PassengerOrDestination,
        Workshop
    }
    public ArrowTarget arrowTarget;

    private Vector2 currentPos;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (player != null)
            currentPos = (Vector2)player.position + Vector2.up * radius;
    }

    void Update()
    {
        if (player == null) return;

        Transform target = null;

        if (arrowTarget == ArrowTarget.PassengerOrDestination)
        {
            if (playerController == null) return;

            if (playerController.HasPassenger())
            {
                target = DestinationManager.Instance?.currentDestination;
                if (sr != null) sr.color = destinationColor;
            }
            else
            {
                target = PassengerManager.Instance?.currentPassenger;
                if (sr != null) sr.color = passengerColor;
            }
        }
        else if (arrowTarget == ArrowTarget.Workshop)
        {
            target = Workshop.Instance?.currentDestination;
            if (sr != null) sr.color = workshopColor;
        }

        if (target == null) return;

        Vector2 dir = ((Vector2)target.position - (Vector2)player.position).normalized;
        Vector2 desiredPos = (Vector2)player.position + dir * radius;

        currentPos = Vector2.Lerp(currentPos, desiredPos, Time.deltaTime * smoothSpeed);

        transform.position = currentPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}