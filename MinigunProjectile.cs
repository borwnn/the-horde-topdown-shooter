using UnityEngine;

public class MinigunProjectile : MonoBehaviour
{
    public float speed = 25f;
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // referenced from link
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
         
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}