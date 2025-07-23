using UnityEngine;
using System.Collections;

public class HealthDome : MonoBehaviour
{
    public int healAmount = 10;        // Amount of health to heal each tick
    public float healFrequency = 1f;   // How often to heal (in seconds)
    public bool playerEntered;
    public int maxHealthDome;

    private PlayerHealth playerHealth; // Reference to the player's health script
    private Coroutine healCoroutine;   // Reference to the running heal coroutine

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Start healing the player
                healCoroutine = StartCoroutine(HealPlayer());
                Debug.Log("Player entered the Health Dome!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop healing when the player leaves
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
                healCoroutine = null;
            }
            playerHealth = null;
            Debug.Log("Player left the Health Dome!");
        }
    }

    private IEnumerator HealPlayer()
    {
        while (true)
        {
            playerHealth.Heal(healAmount); // Call Heal method on player
            maxHealthDome -= healAmount;
            yield return new WaitForSeconds(healFrequency);
        }
    }
}
