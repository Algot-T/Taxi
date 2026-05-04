using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;

    public float roamRadius = 3f;
    public float roamChangeTime = 2f;

    private Rigidbody2D rb;
    private Transform player;

    private Vector2 roamTarget;
    private float roamTimer;

    public bool hasHitPlayer = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
            player = playerGO.transform;

        PickNewRoamTarget();
    }

    void FixedUpdate()
    {
        if (hasHitPlayer)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectionRadius)
            {
                ChasePlayer();
                return;
            }
        }

        Roam();
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void Roam()
    {
        roamTimer -= Time.fixedDeltaTime;

        if (roamTimer <= 0f)
        {
            PickNewRoamTarget();
        }

        Vector2 direction = (roamTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * (moveSpeed * 0.5f);
    }

    void PickNewRoamTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * roamRadius;
        roamTarget = (Vector2)transform.position + randomOffset;

        roamTimer = roamChangeTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitPlayer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;

            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.TakeDamage(1);
            }

            rb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}