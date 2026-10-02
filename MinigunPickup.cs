using UnityEngine;

public class MinigunPickup : MonoBehaviour
{
    public AudioClip pickupSound; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Shooting playerShooting = other.GetComponent<Shooting>();
            
            if (playerShooting != null)
            {
                playerShooting.ActivateMinigun();
                
                if (AudioManager.instance != null && pickupSound != null)
                {
                    AudioManager.instance.PlaySFX(pickupSound);
                }
                
                Destroy(gameObject);
            }
        }
    }
}