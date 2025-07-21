using UnityEngine;

public class throwGrenade : MonoBehaviour
{
    public GameObject beaconPrefab;   // Drag your beacon prefab here in the inspector
    private GameObject currentBeacon; // Holds reference to spawned beacon

    void Update()
    {
        // Spawn beacon at player position (e.g., KeyCode.B)
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBeacon();
        }

        // Teleport to beacon
        if (Input.GetKeyDown(KeyCode.F) && currentBeacon != null)
        {
            TeleportToBeacon();
        }

        // Destroy beacon
        if (Input.GetKeyDown(KeyCode.G) && currentBeacon != null)
        {
            DestroyBeacon();
        }
    }

    void SpawnBeacon()
    {
        if (currentBeacon != null)
        {
            Destroy(currentBeacon);
        }

        currentBeacon = Instantiate(beaconPrefab, transform.position, Quaternion.identity);
    }

    void TeleportToBeacon()
    {
        transform.position = currentBeacon.transform.position;
    }

    void DestroyBeacon()
    {
        Destroy(currentBeacon);
        currentBeacon = null;
    }
}
