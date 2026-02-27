using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    [Header("References")]
    public IndicatorArrow arrow;
    public MoneyUI moneyUI;

    private Rigidbody2D rb;
    private float moveInput;
    private float rotationInput;
    private bool hasPassenger = false;
    private int money = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Tank controls
        moveInput = Input.GetAxis("Vertical");
        rotationInput = -Input.GetAxis("Horizontal");

        UpdateArrowTarget();
    }

    void FixedUpdate()
    {
        // Rotation
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime);

        // Fram/back
        Vector2 movement = transform.up * moveInput * moveSpeed;
        rb.velocity = movement;
    }

    private void UpdateArrowTarget()
    {
        if (arrow == null) return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Plocka upp passagerare
        if (!hasPassenger)
        {
            Passenger passenger = other.GetComponent<Passenger>();
            if (passenger != null)
            {
                passenger.PickUp();
                hasPassenger = true;
                Debug.Log("Passenger picked up by Player!");

                DestinationManager.Instance.SpawnDestination();
                return;
            }
        }

        // Leverera passagerare
        if (hasPassenger)
        {
            Destination destination = other.GetComponent<Destination>();
            if (destination != null)
            {
                destination.Reach();
                hasPassenger = false;

                money += destination.reward;
                Debug.Log("Money: " + money);

                if (moneyUI != null)
                    moneyUI.UpdateMoney(money);

                PassengerManager.Instance.SpawnPassenger();
            }
        }
    }

    public bool HasPassenger()
    {
        return hasPassenger;
    }
}