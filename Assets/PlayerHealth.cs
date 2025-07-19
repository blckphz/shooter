using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int Health = 100; // Start with 100 health
    public CapsuleCollider capsule;
    public GameObject gameOverScreen, gamescreen; // Assign in Inspector
    private stateManager stateManager;
    private Rigidbody rb; // For applying force

    void Start()
    {
        Debug.Log(Health);

        capsule = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>(); // Get Rigidbody for physics force

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Hide at start
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy! Health -20");
            //Health -= 20;

            // Destroy the active grappling hook belonging to this player
            DestroyActiveGrapplingHook();

            // Optionally apply a small upward force to the player (uncomment if you want)
            // rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    void DestroyActiveGrapplingHook()
    {
        // Find all grappling hooks in the scene
        grapplingHook[] hooks = FindObjectsOfType<grapplingHook>();

        foreach (var hook in hooks)
        {
            // Check if the hook belongs to this player
            if (hook.player == this.transform)
            {
                Destroy(hook.gameObject);
                Debug.Log("Destroyed grappling hook on collision with enemy");
            }
        }
    }

    void ShowGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Show Game Over UI
            gamescreen.SetActive(false);
            Health = 100;
        }

        // Optionally stop player movement and shooting
        // Time.timeScale = 0f; // Freeze the game
    }
}
