using UnityEngine;

public class beaconBehav : MonoBehaviour
{
    void Start()
    {
        // You can add effects here like particles
    }

    void Update()
    {
        // Optional: Rotate or do something for visual effect
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }
}
