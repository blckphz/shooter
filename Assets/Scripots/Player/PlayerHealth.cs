using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerHealth : MonoBehaviour
{
    public static int Health = 100; // Start with 100 health
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
            Health -= 20;
            UpdateHealthUI();

            if (Health <= 0)
            {
                ShowGameOver();
            }

            DestroyActiveGrapplingHook();
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
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + Health.ToString();
        }
    }
}
