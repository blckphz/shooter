using UnityEngine;

public class enemyBehav : MonoBehaviour
{
    public float boostBonus = 5f;  // Upward boost force/speed

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Add upward velocity boost
                Vector3 velocity = playerRb.linearVelocity;
                velocity.y = boostBonus;  // directly set upward velocity
                playerRb.linearVelocity = velocity;

                // Alternatively, you could use AddForce:
                // playerRb.AddForce(Vector3.up * boostBonus, ForceMode.VelocityChange);
            }

            Destroy(gameObject);
        }
    }
}
