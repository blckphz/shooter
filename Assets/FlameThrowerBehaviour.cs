using UnityEngine;
using System.Collections;

public class FlameThrowerBehaviour : MonoBehaviour
{
    public FlameThrowerSO flameThrowerSO;
    private Coroutine ammoDrainCoroutine;
    private float ammoDrainAccumulator = 0f;

    // Enemy detection
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered the flamethrower range!");
            // Optionally apply damage or start burning
            ApplyFlameDamage(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Damage over time could be applied here
            Debug.Log("Enemy is within flamethrower range!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy exited the flamethrower range.");
        }
    }

    private void ApplyFlameDamage(GameObject enemy)
    {
        // Example of applying damage if enemy has a health component
        var health = enemy.GetComponent<enemyStats>();
        if (health != null)
        {
            health.TakeDamage(flameThrowerSO.damage);
        }
    }
}
