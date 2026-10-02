using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public int damageToPlayer = 10; // added

    [Header("Stats")]
    [Tooltip("The health of the enemy.")]
    public int health = 1;
    [Tooltip("How fast the enemy moves towards the player.")]
    public float moveSpeed = 2.5f;

    [Header("AI & Visuals")]
    [Tooltip("Adjust this if your sprite isn't facing the player correctly. Try 90 or -90.")]
    public float rotationOffset = 0f;
    [Tooltip("The layer that contains your walls and obstacles.")]
    public LayerMask obstacleLayer;
    [Tooltip("The layer that contains other enemies.")]
    public LayerMask enemyLayer;
    [Tooltip("How far ahead the enemy looks for obstacles.")]
    public float obstacleCheckDistance = 1.0f;
    [Tooltip("The radius for detecting other enemies to avoid clumping.")]
    public float separationRadius = 1.0f;
    [Tooltip("How strongly enemies will try to move away from each other.")]
    public float separationWeight = 2.0f;

    public AudioClip enemy_death;

    private Transform player;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    void FixedUpdate()
    {
        if (player != null && !isDead)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            Vector2 finalMoveDirection = CalculateAvoidanceDirection(directionToPlayer);
            
            rb.linearVelocity = finalMoveDirection * moveSpeed;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        }
    }
    
    private Vector2 CalculateAvoidanceDirection(Vector2 directionToPlayer)
    {
        return directionToPlayer; 
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.EnemyKilled();
        }
          if (AudioManager.instance != null && enemy_death != null) // added
                {
                    AudioManager.instance.PlaySFX(enemy_death);
                }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision) // added code for PlayerHealth
    {
        if (!isDead && collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
            else
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.GameOver();
                }
            }

        }
    }
    
   
}


