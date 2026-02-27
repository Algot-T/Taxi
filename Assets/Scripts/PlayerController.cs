using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;
    [SerializeField] private HealthUI healthUI;

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

    void Start()
    {
        // Init health
        currentHealth = maxHealth;

        if (HealthUI.Instance != null)
            HealthUI.Instance.UpdateHealth(currentHealth, maxHealth);
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

        // Movement fram/back
        Vector2 movement = transform.up * moveInput * moveSpeed;
        rb.velocity = movement;
    }

    private void UpdateArrowTarget()
    {
        if (arrow == null) return;
        // Arrow använder redan PlayerController.HasPassenger()
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // === PICKUP PASSENGER ===
        if (!hasPassenger)
        {
            Passenger passenger = other.GetComponent<Passenger>();
            if (passenger != null)
            {
                passenger.PickUp();
                hasPassenger = true;

                Debug.Log("Passenger picked up!");

                DestinationManager.Instance.SpawnDestination();
                return;
            }
        }

        // === REACH DESTINATION ===
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

    // =========================
    // HEALTH SYSTEM
    // =========================

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log("Taxi hit! Health: " + currentHealth);

        if (HealthUI.Instance != null)
            HealthUI.Instance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        rb.velocity = Vector2.zero;

        // Visa Game Over UI
        GameOverManager.Instance.ShowGameOver();
    }

    // =========================

    public bool HasPassenger()
    {
        return hasPassenger;
    }
}