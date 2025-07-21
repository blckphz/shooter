using UnityEngine;

public class SlideBox : MonoBehaviour
{
    public float slipperyDrag = 0.2f;       // Low drag for sliding
    public float normalDrag = 5f;            // Normal drag when off ice
    public float speedMultiplier = 1.5f;     // How much faster player moves on this ice

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (player != null && playerRb != null)
            {
                player.SetIceState(true, speedMultiplier);
                playerRb.linearDamping = slipperyDrag;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (player != null && playerRb != null)
            {
                player.SetIceState(false, 1f); // Reset multiplier to 1 when off ice
                playerRb.linearDamping = normalDrag;
            }
        }
    }
}
