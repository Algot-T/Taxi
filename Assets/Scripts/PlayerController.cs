using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 200f;
    private float speedModifier = 1f;

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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

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

        HandleDestinationCheck();

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Spelaren är just nu på: " + transform.position);
        }
    }

    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);

        float targetModifier = (hit != null && hit.CompareTag("Roads")) ? 1f : 0.7f;

        speedModifier = Mathf.Lerp(speedModifier, targetModifier, Time.fixedDeltaTime * 5f);

        float finalSpeed = moveSpeed * speedModifier;

        rb.MovePosition(rb.position + (Vector2)transform.up * moveInput * finalSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime);
    }

    void HandleDestinationCheck()
    {
        if (!HasPassenger()) return;

        if (DestinationManager.Instance != null)
        {
            float dist = Vector2.Distance(transform.position, DestinationManager.Instance.currentDestination);

            if (dist < 1.5f)
            {
                DeliverPassenger();
                PassengerManager.Instance.SpawnPassenger();
            }
        }
    }

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

                if (DestinationManager.Instance != null)
                    DestinationManager.Instance.SetRandomDestination();

                return;
            }
        }
    }

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

    void GameOver()
    {
        rb.velocity = Vector2.zero;

        if (GameOverManager.Instance != null)
            GameOverManager.Instance.ShowGameOver();
    }

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