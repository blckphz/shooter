using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerHealth : MonoBehaviour
{
    public static int Health = 100; // Start with 100 health
    public int maxHealth = 100;     // Maximum health cap
    public CapsuleCollider capsule;
    public TextMeshProUGUI healthText; // Reference to TextMeshPro text

    private stateManager stateManager;
    private Rigidbody rb; // For applying force

    void Start()
    {
        if (healthText == null)
        {
            healthText = GameObject.Find("hp").GetComponent<TextMeshProUGUI>();
        }

        Debug.Log(Health);

        capsule = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>(); // Get Rigidbody for physics force

        UpdateHealthUI(); // Initialize UI
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Get the enemyMove component to check if hooked
            enemyMove enemy = other.gameObject.GetComponent<enemyMove>();

            if (enemy != null && enemy.grapped)
            {
                // Enemy is hooked - do NOT damage player
                Debug.Log("Collided with hooked enemy - no damage taken.");
                return;
            }

            // Enemy is NOT hooked - apply damage
            Debug.Log("Collided with Enemy! Health -20");
            ChangeHealth(-20);

            DestroyActiveGrapplingHook();
        }
    }

    // New method to safely change health (positive or negative)
    public void ChangeHealth(int amount)
    {
        Health += amount;

        // Clamp health between 0 and maxHealth
        Health = Mathf.Clamp(Health, 0, maxHealth);

        UpdateHealthUI();

        if (Health <= 0)
        {
            ShowGameOver();
        }
    }

    // Convenience method to heal the player by a positive amount
    public void Heal(int amount)
    {
        if (amount > 0)
        {
            ChangeHealth(amount);
            Debug.Log("Healed by " + amount + ". Current health: " + Health);
        }
    }

    public void DestroyActiveGrapplingHook()
    {
        grapplingHook[] hooks = FindObjectsOfType<grapplingHook>();

        foreach (var hook in hooks)
        {
            if (hook.player == this.transform)
            {
                Destroy(hook.gameObject);
            }
        }
    }

    public void ShowGameOver()
    {
        // Your game over logic here
        Debug.Log("Game Over!");
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + Health.ToString();
        }
    }
}
