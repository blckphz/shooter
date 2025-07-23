using UnityEngine;

public class rayCastScript : MonoBehaviour
{
    public float rayDistance = 100f; // Length of the ray

    void Update()
    {
        // Define the ray direction (forward from this GameObject)
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        // Draw the ray in the Scene view (red color)
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

        // Perform the Raycast
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
        {
            // Check if the hit object has the "Enemy" tag
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Balloon"))
            {
                Debug.Log("Ray hit an Enemy: " + hit.collider.name);

            }
        }
    }
}
 