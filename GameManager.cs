using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private UIManager uiManager;

    [Header("UI References")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;

    [Header("Player and Enemy References")]
    public Transform playerTransform;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    [Header("Wave Management")]
    public int maxWaves = 6;
    public int baseEnemiesPerWave = 5;
    public int increaseEnemiesPerWave = 2;
    public float timeBetweenWaves = 3f;

    [Header("Power-ups")]
    public GameObject shotgunPowerupPrefab;
    public GameObject smgPowerupPrefab;
    public GameObject minigunPowerupPrefab;

    public GameObject healthPickupPrefab;
    [Tooltip("Chance (0.0 to 1.0) for an enemy to drop a health pickup.")]
    public float healthPickupDropChance = 0.1f;

    [Header("Spawn Area")]
    public float minX = -20f, maxX = 20f, minY = -15f, maxY = 15f;
    public float spawnDistanceFromPlayer = 10f;
    private int currentWave = 0;
    private int enemiesRemaining;
    private bool isSpawning = false;
    private PlayerHealth playerHealth;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager == null) Debug.LogError("GameManager could not find the UIManager!");
    }

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
        if (playerTransform != null)
        {
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
            if (playerHealth == null) Debug.LogError("GameManager Start: Player missing PlayerHealth script!");
        }
        else Debug.LogError("GameManager Start: Could not find Player Transform!");

        StartNextWave();
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;

        if (healthPickupPrefab != null && Random.value <= healthPickupDropChance)
        {
            SpawnHealthPickup();
        }

        UpdateUI();
        if (enemiesRemaining <= 0 && !isSpawning)
        {
            if (currentWave >= maxWaves) { GameWin(); return; }
            StartCoroutine(WaveCooldown());
        }
    }

    private IEnumerator WaveCooldown()
    {
        isSpawning = true;

        if (currentWave % 2 == 0)
        {
            if (shotgunPowerupPrefab != null) SpawnShotgunPowerup();
            if (smgPowerupPrefab != null) SpawnSMGPowerup();
        }

        if (currentWave >= 1)
        {
            if (minigunPowerupPrefab != null) SpawnMinigunPowerup();
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }

    public void SaveGame()
    {
        if (playerHealth == null) { Debug.LogError("Cannot save: PlayerHealth script not found!"); return; }

        Debug.Log("Saving game state...");

        List<EnemyData> activeEnemies = new List<EnemyData>();
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            activeEnemies.Add(new EnemyData(enemy.health, enemy.transform.position));

        List<PlayerData.PowerUpData> activePowerUps = new List<PlayerData.PowerUpData>();
        foreach (var powerup in FindObjectsByType<ShotgunPowerup>(FindObjectsSortMode.None))
            activePowerUps.Add(new PlayerData.PowerUpData(powerup.transform.position));

        List<PlayerData.SMGPowerupData> activeSMGPowerups = new List<PlayerData.SMGPowerupData>();
        foreach (var smgPickup in FindObjectsByType<SMGPowerup>(FindObjectsSortMode.None))
            activeSMGPowerups.Add(new PlayerData.SMGPowerupData(smgPickup.transform.position));


        List<PlayerData.MinigunPowerupData> activeMinigunPowerups = new List<PlayerData.MinigunPowerupData>();
        foreach (var minigunPickup in FindObjectsByType<MinigunPickup>(FindObjectsSortMode.None))
        {
            activeMinigunPowerups.Add(new PlayerData.MinigunPowerupData(minigunPickup.transform.position));
        }

        List<PlayerData.HealthPickupData> activeHealthPickups = new List<PlayerData.HealthPickupData>();
        foreach (var pickup in FindObjectsByType<HealthPickup>(FindObjectsSortMode.None))
            activeHealthPickups.Add(new PlayerData.HealthPickupData(pickup.transform.position));

        PlayerData data = new PlayerData(
            playerHealth.GetCurrentHealth(),
            playerTransform.position,
            currentWave,
            enemiesRemaining,
            activeEnemies,
            activePowerUps,
            activeHealthPickups,
            activeSMGPowerups,
            activeMinigunPowerups
        );

        SaveSystem.SaveGame(data);
        Debug.Log("Game Saved Successfully!");
    }

    public void LoadGame()
    {
        Debug.Log("Loading game state...");
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            if (playerTransform == null || playerHealth == null) { return; }

            foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None)) Destroy(enemy.gameObject);
            foreach (var powerup in FindObjectsByType<ShotgunPowerup>(FindObjectsSortMode.None)) Destroy(powerup.gameObject);
            foreach (var smgPickup in FindObjectsByType<SMGPowerup>(FindObjectsSortMode.None)) Destroy(smgPickup.gameObject);
            foreach (var minigunPickup in FindObjectsByType<MinigunPickup>(FindObjectsSortMode.None)) Destroy(minigunPickup.gameObject); 
            foreach (var pickup in FindObjectsByType<HealthPickup>(FindObjectsSortMode.None)) Destroy(pickup.gameObject);

            playerTransform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            playerHealth.SetHealth(data.playerHealth);
            currentWave = data.currentWave;
            enemiesRemaining = data.enemiesRemaining;
            
            if (playerTransform.GetComponent<Shooting>() != null) 
                playerTransform.GetComponent<Shooting>().ResetToDefaultWeapon();

            if (data.activeEnemies != null)
            {
                foreach (var enemyData in data.activeEnemies)
                {
                    Vector3 position = new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]);
                    GameObject enemyObj = Instantiate(enemyPrefab, position, Quaternion.identity);
                    Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                    enemyScript.health = (int)enemyData.health;
                    enemyScript.SetPlayer(playerTransform);
                }
            }

            if (data.activePowerUps != null && shotgunPowerupPrefab != null)
            {
                foreach (var powerUpData in data.activePowerUps)
                {
                    Instantiate(shotgunPowerupPrefab, new Vector3(powerUpData.position[0], powerUpData.position[1], powerUpData.position[2]), Quaternion.identity);
                }
            }
            if (data.activeSMGPowerups != null && smgPowerupPrefab != null)
            {
                foreach (var smgData in data.activeSMGPowerups)
                {
                    Instantiate(smgPowerupPrefab, new Vector3(smgData.position[0], smgData.position[1], smgData.position[2]), Quaternion.identity);
                }
            }
            if (data.activeMinigunPowerups != null && minigunPowerupPrefab != null)
            {
                foreach (var minigunData in data.activeMinigunPowerups)
                {
                    Instantiate(minigunPowerupPrefab, new Vector3(minigunData.position[0], minigunData.position[1], minigunData.position[2]), Quaternion.identity);
                }
            }
            if (data.activeHealthPickups != null && healthPickupPrefab != null)
            {
                 foreach (var pickupData in data.activeHealthPickups)
                {
                    Instantiate(healthPickupPrefab, new Vector3(pickupData.position[0], pickupData.position[1], pickupData.position[2]), Quaternion.identity);
                }
             }

            UpdateUI();

            if (enemiesRemaining <= 0) // Original code
            {
                if (currentWave >= maxWaves)
                {
                    GameWin();
                }
                else
                {
                    Debug.Log("Loaded during wave cooldown. Restarting next wave timer...");
                    StartCoroutine(WaveCooldown());
                }
            }
            Debug.Log("Game Loaded Successfully!");
        }
    
    }

    public void GameOver() { if (uiManager != null) uiManager.ShowGameOver(); }
    private void StartNextWave() { currentWave++; isSpawning = true; if (currentWave == maxWaves) SpawnBoss(); else { int enemiesToSpawn = baseEnemiesPerWave + ((currentWave - 1) * increaseEnemiesPerWave); enemiesRemaining = enemiesToSpawn; for (int i = 0; i < enemiesToSpawn; i++) SpawnEnemy(enemyPrefab); } isSpawning = false; UpdateUI(); }
    private void UpdateUI() { if (waveText != null) waveText.text = "Wave: " + currentWave + "/" + maxWaves; if (enemiesLeftText != null) enemiesLeftText.text = "Enemies Left: " + enemiesRemaining; }
    private void SpawnEnemy(GameObject prefab) { if (playerTransform == null || prefab == null) return; Vector2 spawnPos; do { spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)); } while (Vector2.Distance(spawnPos, playerTransform.position) < spawnDistanceFromPlayer); GameObject enemyObj = Instantiate(prefab, spawnPos, Quaternion.identity); Enemy enemyScript = enemyObj.GetComponent<Enemy>(); if (enemyScript != null) enemyScript.SetPlayer(playerTransform); }
    private void SpawnBoss() { enemiesRemaining = 1; SpawnEnemy(bossPrefab); }
    private void SpawnShotgunPowerup() { Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)); Instantiate(shotgunPowerupPrefab, spawnPos, Quaternion.identity); }
    private void SpawnSMGPowerup() { Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)); Instantiate(smgPowerupPrefab, spawnPos, Quaternion.identity); Debug.Log("SMG pickup spawned!"); }

    private void SpawnMinigunPowerup()
    {
        Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));    // ORIGINAL CODE
        Instantiate(minigunPowerupPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Minigun pickup spawned!");
    }

    private void GameWin() { Debug.Log("VICTORY!"); if (uiManager != null) uiManager.ShowGameWin(); else Time.timeScale = 0f; }
    private void SpawnHealthPickup() { Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)); Instantiate(healthPickupPrefab, spawnPos, Quaternion.identity); Debug.Log("Health pickup spawned!"); }
// VISUALIZE SPAWN AREA
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Calculate center and size of the spawn box
        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
        
        // Draw the box
        Gizmos.DrawWireCube(center, size);
    }
}