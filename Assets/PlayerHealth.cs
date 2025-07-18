using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int Health = 100; // Start with 100 health
    public CapsuleCollider capsule;
    public GameObject gameOverScreen, gamescreen; // Assign in Inspector
    private stateManager stateManager;


    void Start()
    {

        Debug.Log(Health);

        capsule = GetComponent<CapsuleCollider>();
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

            Destroy(other.gameObject);

            if (Health <= 0)
            {
                Debug.Log("Player Died!");
                ShowGameOver();
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
        //Time.timeScale = 0f; // Freeze the game
    }
}
