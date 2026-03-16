using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 200f;

    private float moveInput;
    private float rotationInput;
    private Rigidbody2D rb;

    [Header("Taxi Stats")]
    public int maxHP = 5;
    public int passengerReward = 20;
    
    public int money = 0;
    public int currentHP;

    [Header("Health UI")]
    public HealthUI healthUI;

    [Header("Passenger Tracking")]
    private bool hasPassenger = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        Vector2 center = new Vector2(MapManager.Instance.Width / 2f, MapManager.Instance.Height / 2f);
        transform.position = center;

        if (healthUI != null)
        {
            healthUI.SetMaxHealth(maxHP);
            healthUI.SetHealth(currentHP);
        }

        if (MoneyUI.Instance != null)
            MoneyUI.Instance.UpdateMoney(money);
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        rotationInput = -Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Vector2)transform.up * moveInput * moveSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime);
    }

    // =======================
    // Passenger / Arrow System
    // =======================
    public bool HasPassenger()
    {
        return hasPassenger;
    }

    public void PickUpPassenger()
    {
        hasPassenger = true;
    }

    public void DeliverPassenger()
    {
        hasPassenger = false;
        AddMoneyFromPassenger();
    }

    public void AddMoneyFromPassenger()
    {
        money += passengerReward;

        if (MoneyUI.Instance != null)
            MoneyUI.Instance.UpdateMoney(money);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!HasPassenger())
        {
            Passenger passenger = other.GetComponent<Passenger>();
            if (passenger != null)
            {
                passenger.PickUp();
                PickUpPassenger();
                DestinationManager.Instance.SpawnDestination();
                return;
            }
        }

        if (HasPassenger())
        {
            Destination destination = other.GetComponent<Destination>();
            if (destination != null)
            {
                destination.Reach();
                DeliverPassenger();
                PassengerManager.Instance.SpawnPassenger();
            }
        }

    }

    // =======================
    // Damage / Health System
    // =======================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null && !enemy.hasHitPlayer)
            {
                TakeDamage(1);
                enemy.hasHitPlayer = true;
                Destroy(collision.gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (healthUI != null)
            healthUI.SetHealth(currentHP);

        if (currentHP <= 0)
            GameOver();
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        rb.velocity = Vector2.zero;

        if (GameOverManager.Instance != null)
            GameOverManager.Instance.ShowGameOver();
    }

    // =======================
    // Workshop / Upgrades
    // =======================
    public void HealFull()
    {
        currentHP = maxHP;
        if (healthUI != null)
            healthUI.SetHealth(currentHP);
    }

    public void UpgradeHP()
    {
        maxHP += 1;
        currentHP = maxHP;
        if (healthUI != null)
        {
            healthUI.SetMaxHealth(maxHP);
            healthUI.SetHealth(currentHP);
        }
    }

    public void UpgradeSpeed()
    {
        moveSpeed *= 1.05f;
    }

    public void UpgradePassengerReward()
    {
        passengerReward += 10;
    }
}