using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn, pickUp;   // Assign in Inspector
    public float spawnInterval = 2f;   // Time between spawns

    [Header("Random Position Ranges")]
    public Vector2 xRange = new Vector2(-100f, 0f);
    public Vector2 zRange = new Vector2(-100f, 0f);

    public float fixedY = 0f;           // Fixed Y position

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;
        }
    }

    void SpawnPrefab()
    {
        float randomX = Random.Range(xRange.x, xRange.y);
        float randomZ = Random.Range(zRange.x, zRange.y);

        Vector3 spawnPos = new Vector3(randomX, fixedY, randomZ);
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}
