using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;

    private Rigidbody2D rb;
    private Transform player;
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
    }

    void FixedUpdate()
    {
        if (player == null || hasHitPlayer)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= detectionRadius)
        {
            direction.Normalize();
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
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