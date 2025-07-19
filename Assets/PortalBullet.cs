using UnityEngine;

public class PortalBullet : MonoBehaviour
{
    private PortalGun portalGun;
    private bool isBlue;
    private Collider bulletCollider;

    private bool hasPlacedPortal = false;  // Track if portal is already placed

    private void Awake()
    {
        bulletCollider = GetComponent<Collider>();
        if (bulletCollider != null)
        {
            bulletCollider.enabled = true;  // Enable collider immediately
            Debug.Log("Bullet collider enabled instantly");
        }
    }

    public void Initialize(PortalGun gun, bool blue)
    {
        portalGun = gun;
        isBlue = blue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet collided with {collision.collider.name} at {Time.time}");

        if (!bulletCollider.enabled)
        {
            Debug.Log("Ignoring collision: collider disabled");
            return;
        }

        // --- Ignore Player but continue flying ---
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Ignoring collision with Player and continuing...");
            Physics.IgnoreCollision(collision.collider, bulletCollider);
            return;  // Don't place portal, just ignore this hit
        }

        // --- Ignore non-portal surfaces ---
        string surfaceName = collision.collider.gameObject.name;
        if (!surfaceName.Contains("Wall") && !surfaceName.Contains("Surface"))
        {
            Debug.Log($"Ignoring collision with non-wall surface: {surfaceName}");
            return;
        }

        // --- Portal placement ---
        if (hasPlacedPortal)
        {
            Debug.Log("Ignoring collision: portal already placed");
            return;
        }

        hasPlacedPortal = true;
        bulletCollider.enabled = false;  // Prevent further collisions
        Debug.Log("Bullet collider disabled after placing portal");

        ContactPoint contact = collision.contacts[0];
        Vector3 portalPosition = contact.point;
        Quaternion portalRotation = Quaternion.LookRotation(contact.normal);

        if (portalGun != null)
        {
            Debug.Log($"Placing {(isBlue ? "Blue" : "Orange")} portal at position {portalPosition}");
            portalGun.PlacePortal(portalPosition, portalRotation, isBlue);
        }

        Destroy(gameObject);
        Debug.Log("Bullet destroyed after placing portal");
    }

}
