using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class ShotgunPowerup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // modified
            Shooting playerShooting = other.GetComponent<Shooting>();
            AudioManager.instance.PlaySFX(AudioManager.instance.shotgun_pickup);

            if (playerShooting != null)
            {
                playerShooting.ActivateShotgun();
                
                Destroy(gameObject);
            }
        }
    }
}
