using UnityEngine;

public class ColliderGameOver : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding is the player (adjust tag or layer as needed)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset score to 0
            Scorescript.score = 0;

            // Debug message
            Debug.Log("Floor touched - Score reset to 0");
        }
    }
}
