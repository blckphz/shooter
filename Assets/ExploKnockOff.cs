using UnityEngine;

public class ExploKnockOff : MonoBehaviour
{
    public float launchForce = 10f; // force of the launch
    public Vector3 launchDirection = Vector3.up + Vector3.forward; // example direction

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the hitbox is player or enemy
        // You can check by tag or component � assuming tags "Player" and "Enemy" for now

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Reset velocity for consistent knockback
                rb.linearVelocity = Vector3.zero;
                // Apply launch force in the direction specified
                rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);
            }
        }
    }
}