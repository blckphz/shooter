using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject prefabToSpawn;   // Spawns at fixedY
    public GameObject pickUp;          // Spawns on ground
    public GameObject extraObject;     // Another ground object

    [Header("Spawn Intervals")]
    public float mainSpawnInterval = 2f;  // Time between spawns for prefabToSpawn
    public float pickUpSpawnInterval = 5f; // Time between spawns for pickUp
    public float extraObjectInterval = 7f; // Time between spawns for extraObject

    [Header("Random Position Ranges")]
    public Vector2 xRange = new Vector2(-100f, 0f);
    public Vector2 zRange = new Vector2(-100f, 0f);

    public float fixedY = 5f;  // Height for main prefab

    private float mainTimer = 0f;
    private float pickUpTimer = 0f;
    private float extraTimer = 0f;

    void Update()
    {
        mainTimer += Time.deltaTime;
        pickUpTimer += Time.deltaTime;
        extraTimer += Time.deltaTime;

        if (mainTimer >= mainSpawnInterval)
        {
            SpawnPrefab(prefabToSpawn, fixedY);
            mainTimer = 0f;
        }

        if (pickUpTimer >= pickUpSpawnInterval)
        {
            SpawnPrefab(pickUp, 0f);
            pickUpTimer = 0f;
        }

        if (extraTimer >= extraObjectInterval)
        {
            SpawnPrefab(extraObject, 0f);
            extraTimer = 0f;
        }
    }

    void SpawnPrefab(GameObject prefab, float yPos)
    {
        if (prefab == null) return; // Safety check

        float randomX = Random.Range(xRange.x, xRange.y);
        float randomZ = Random.Range(zRange.x, zRange.y);

        Vector3 spawnPos = new Vector3(randomX, yPos, randomZ);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
