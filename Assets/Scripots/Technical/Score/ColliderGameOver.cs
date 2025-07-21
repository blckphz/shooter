using UnityEngine;

public class ColliderGameOver : MonoBehaviour
{
    public bool isGrounded = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGrounded = false;
        }
    }
}
