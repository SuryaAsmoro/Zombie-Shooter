using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    [SerializeField] MonoBehaviour factory;
    IFactory Factory { get { return factory as IFactory; }
    }

    private void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
            return;

        Debug.Log("ENemy Spawned");
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int spawnEnemy = Random.Range(0, 3);

        Factory.FactoryMethod(spawnEnemy, spawnPoints[spawnPointIndex]);
    }
}
