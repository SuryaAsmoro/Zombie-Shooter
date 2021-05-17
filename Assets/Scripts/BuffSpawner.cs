using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public float spawnTime = 1f;
    public Transform[] spawnPoints;
    public GameObject[] buffs;

    public float positionOffset = 5f;

    private void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int spawnBuff = Random.Range(0, buffs.Length);
        Vector3 offset = new Vector3(Random.Range(-positionOffset, positionOffset), 0.8f, Random.Range(-positionOffset, positionOffset));

        Instantiate(buffs[spawnBuff], spawnPoints[spawnPointIndex].localPosition + offset, transform.rotation, spawnPoints[spawnPointIndex]);
    }
}
