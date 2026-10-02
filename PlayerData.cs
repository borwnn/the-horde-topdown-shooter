using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    [System.Serializable]
    public class PowerUpData
    {
        public float[] position;
        public PowerUpData(Vector3 powerUpPosition)
        {
            position = new float[3];
            position[0] = powerUpPosition.x;
            position[1] = powerUpPosition.y;
            position[2] = powerUpPosition.z;
        }
    }

    [System.Serializable]
    public class SMGPowerupData
    {
        public float[] position;
        public SMGPowerupData(Vector3 powerUpPosition)
        {
            position = new float[3];
            position[0] = powerUpPosition.x;
            position[1] = powerUpPosition.y;
            position[2] = powerUpPosition.z;
        }
    }

     [System.Serializable]
    public class MinigunPowerupData // slightly modified code - referenced
    {
        public float[] position;
        public MinigunPowerupData(Vector3 powerUpPosition)
        {
            position = new float[3];
            position[0] = powerUpPosition.x;
            position[1] = powerUpPosition.y;
            position[2] = powerUpPosition.z;
        }
    }

    [System.Serializable]
    public class HealthPickupData
    {
        public float[] position;
        public HealthPickupData(Vector3 pickupPosition)
        {
            position = new float[3];
            position[0] = pickupPosition.x;
            position[1] = pickupPosition.y;
            position[2] = pickupPosition.z;
        }
    }

    public int playerHealth;
    public float[] playerPosition;
    public int currentWave;
    public int enemiesRemaining;
    public List<EnemyData> activeEnemies;
    public List<PowerUpData> activePowerUps; 
    public List<HealthPickupData> activeHealthPickups;
    public List<SMGPowerupData> activeSMGPowerups;

    public List<MinigunPowerupData> activeMinigunPowerups; // slightly modified 

    public PlayerData(int health, Vector3 position, int wave, int remaining,
                      List<EnemyData> enemies, List<PowerUpData> powerUps,
                      List<HealthPickupData> healthPickups, List<SMGPowerupData> smgPowerups, List<MinigunPowerupData> MinigunPowerups)
    {
        playerHealth = health;
        playerPosition = new float[3];
        playerPosition[0] = position.x;
        playerPosition[1] = position.y;
        playerPosition[2] = position.z;
        currentWave = wave;
        enemiesRemaining = remaining;
        activeEnemies = enemies;
        activePowerUps = powerUps;
        activeHealthPickups = healthPickups;
        activeSMGPowerups = smgPowerups;
        activeMinigunPowerups = MinigunPowerups;
    }
}

