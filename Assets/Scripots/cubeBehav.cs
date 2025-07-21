using UnityEngine;

public class cubeBehav : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpStrength = 8f;
    public bool destroyOnHit = true;
    public float activationDelay = 0.5f; // Delay before it can trigger (0.5 seconds)

    private bool isActive = false;
    private Collider cubeCollider;

    void Start()
    {
        // Rotate by 90 degrees on X-axis when spawned
        transform.Rotate(0f, 90f, 0f);

        cubeCollider = GetComponent<Collider>();
        if (cubeCollider != null) cubeCollider.enabled = false;

        // Enable after delay
        Invoke(nameof(ActivateMine), activationDelay);
    }

    void ActivateMine()
    {
        isActive = true;
        if (cubeCollider != null) cubeCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Reset horizontal velocity
                Vector3 velocity = rb.linearVelocity;
                velocity.y = 0;
                rb.linearVelocity = velocity;

                rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            }

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}
