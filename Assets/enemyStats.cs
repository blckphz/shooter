using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class enemyStats : MonoBehaviour
{
    public int enemyHp = 100;  // Enemy starting HP

    public TextMeshProUGUI healthText;  // Reference to the TextMeshPro UI text component

    void Start()
    {
        UpdateHealthText();
    }

    // Call this function to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        enemyHp -= damage;
        Debug.Log("Enemy took damage: " + damage + ", HP left: " + enemyHp);

        UpdateHealthText();

        if (enemyHp <= 0)
        {
            Die();
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + enemyHp;
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        // You can add death animation, sound, drop loot, destroy enemy, etc.
        Destroy(gameObject);
    }
}
