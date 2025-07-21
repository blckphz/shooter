using UnityEngine;

public class Saver : MonoBehaviour
{
    private SaveScript saveManager;

    [Header("Player Setup")]
    public GameObject playerPrefab;   // Assign your player prefab in Inspector
    private GameObject playerInstance;

    void Start()
    {
        saveManager = SaveScript.Instance;
        saveManager.Load();
        RespawnPlayer();
    }

    void Update()
    {
        if (playerInstance != null)
        {
            UpdateSavedPosition();

            // Save when pressing P
            if (Input.GetKeyDown(KeyCode.P))
            {
                saveManager.Save();
                Debug.Log("Player position saved.");
            }

            // Load when pressing L
            if (Input.GetKeyDown(KeyCode.L))
            {
                saveManager.Load();
                RespawnPlayer();
                Debug.Log("Player position loaded.");
            }
        }
    }

    void SpawnPlayerAtSavedLocation()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not assigned in Saver.");
            return;
        }

        Vector3 spawnPos = new Vector3(
            saveManager.data.playerX,
            saveManager.data.playerY,
            saveManager.data.playerZ
        );

        if (spawnPos == Vector3.zero)
        {
            spawnPos = Vector3.zero;  // Or any default spawn position
            Debug.Log("No saved position found. Spawning at default position (0,0,0).");
        }
        else
        {
            Debug.Log($"Spawning player at saved position: {spawnPos}");
        }

        playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }

    void RespawnPlayer()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }
        SpawnPlayerAtSavedLocation();
    }

    void UpdateSavedPosition()
    {
        if (saveManager == null || playerInstance == null)
            return;

        Transform playerTransform = playerInstance.transform;
        saveManager.data.playerX = playerTransform.position.x;
        saveManager.data.playerY = playerTransform.position.y;
        saveManager.data.playerZ = playerTransform.position.z;
    }
}
