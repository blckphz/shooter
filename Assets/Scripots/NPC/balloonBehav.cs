using UnityEngine;

public class balloonBehav : MonoBehaviour
{
    public float boostBonus = 5f;  // Upward boost force/speed

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Add upward velocity boost (additive or override)
                Vector3 velocity = playerRb.linearVelocity;
                velocity.y = boostBonus;
                playerRb.linearVelocity = velocity;

                // Optionally, interrupt dash so player gets full boost
                PlayerMove pm = other.gameObject.GetComponent<PlayerMove>();
                if (pm != null)
                {
                    pm.ResetDash();
                }
            }

            Destroy(gameObject);
        }
    }
}
