using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SMGPowerup : MonoBehaviour
{
    public AudioClip pickupSound; 

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Shooting shootingScript = other.GetComponent<Shooting>();
            if (shootingScript != null)
            {
                shootingScript.ActivateSMG(); 
                
                if (AudioManager.instance != null && pickupSound != null)
                {
                    AudioManager.instance.PlaySFX(pickupSound);
                }
                
                Destroy(gameObject); 
            }
        }
    }
}
