using UnityEngine;
[System.Serializable]
public class EnemyData
{
    public int health;
    public float[] position;

    public EnemyData(int enemyHealth, Vector3 enemyPosition)
    {
        health = enemyHealth;
        position = new float[3];
        position[0] = enemyPosition.x;
        position[1] = enemyPosition.y;
        position[2] = enemyPosition.z;
    }
}

