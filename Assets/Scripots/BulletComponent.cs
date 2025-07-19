using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        // Check if collided object has tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            enemyStats enemy = other.GetComponent<enemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);  // Destroy bullet on hit enemy
        }
    }

    void Start()
    {
        transform.Rotate(90f, 0f, 0f);
    }
}
