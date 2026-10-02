using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("The speed at which the projectile travels.")]
    public float speed = 20f;
    [Tooltip("The damage this projectile deals.")]
    public int damage = 1;

    private Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        UnityEngine.Object.Destroy(gameObject, 2f);
    }


    public void SetVelocity(Vector2 direction)
    {

        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // modified for shotgun pickup
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Projectile") || hitInfo.CompareTag("Pickup"))
        {
            return;
        }

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        UnityEngine.Object.Destroy(gameObject);
    }
}

