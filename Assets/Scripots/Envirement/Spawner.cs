using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int currentRound = 1;

    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public Transform spawnPoint;

    public List<GameObject> enemies = new List<GameObject>();

    private bool isActiveRound = false;
    private bool hasSpawnedThisRound = false;

    void Update()
    {
        // Only remove null entries — don't clear the list completely
        enemies.RemoveAll(enemy => enemy == null);

        // Round ends ONLY if all enemies are dead AND the correct number were spawned
        if (isActiveRound && enemies.Count == 0 && hasSpawnedThisRound)
        {
            Debug.Log("Round " + currentRound + " complete!");

            isActiveRound = false;
            hasSpawnedThisRound = false;

         
        }

        // Start a new round if ready
        if (!isActiveRound && !hasSpawnedThisRound)
        {
            SpawnAllEnemies();
            hasSpawnedThisRound = true;
            isActiveRound = true;
        }
    }

    void SpawnAllEnemies()
    {
        Vector3 position = spawnPoint ? spawnPoint.position : transform.position;

        enemies.Clear(); // Prepare for a fresh round

        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            enemies.Add(newEnemy);

            currentRound++;
            maxEnemies += 2; // Optional difficulty ramp
        }

        Debug.Log("Spawned " + maxEnemies + " enemies for round " + currentRound);
    }
}
