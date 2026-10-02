using UnityEngine;
// referenced from https://www.youtube.com/watch?v=pgvj6fcurbw&t=199s and shotgun powerup script with modifications
// slightly modified
[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    // added to incorporate healing features
    [Tooltip("How much health this pickup restores.")]
    public int healthAmount = 10;
    [Tooltip("Sound effect to play on pickup")]
    public AudioClip pickupSound;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
        else Debug.LogError("Health Pickup prefab is missing a Collider2D component!", this);
    }

    // referenced from shotgun powerup script
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healthAmount);
                if (AudioManager.instance != null && pickupSound != null)
                {
                    AudioManager.instance.PlaySFX(pickupSound);
                }
                Destroy(gameObject);
            }
        }
    }
}

