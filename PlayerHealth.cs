using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References (Optional)")]
    public Slider healthSlider;
    public Image healthFillImage;
    public TextMeshProUGUI healthText;

    [Header("Damage Feedback (Optional)")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public float damageFlashDuration = 0.1f;
    private SpriteRenderer playerSpriteRenderer;
    private Color originalSpriteColor;
    public AudioClip take_damage; // added

    public AudioClip death_sound; // added


    void Awake()
    {
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            originalSpriteColor = playerSpriteRenderer.color;
        }
    }

    void Start()
    {
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (playerSpriteRenderer != null)
        {
            
        if (AudioManager.instance != null && take_damage != null) // added to include audio
        {
            AudioManager.instance.PlaySFX(take_damage);
        }
            StartCoroutine(DamageFlash());
        }
    }


    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
    }

    void Die()
    {
        if (AudioManager.instance != null && death_sound != null) // added to include audio
        {
            AudioManager.instance.PlaySFX(death_sound);
        }
        Debug.Log("Player Died!");
        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        if (healthFillImage != null && healthSlider != null)
        {
            healthFillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, (float)currentHealth / maxHealth);
        }
        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }
    }

    private IEnumerator DamageFlash()
    {
        if (playerSpriteRenderer == null) yield break;
        playerSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        playerSpriteRenderer.color = originalSpriteColor;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }
}

