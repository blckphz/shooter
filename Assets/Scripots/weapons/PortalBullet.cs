using UnityEngine;

public class PortalBullet : MonoBehaviour
{
    private PortalGun portalGun;
    private bool isBlue;
    public Collider bulletCollider;

    private bool hasPlacedPortal = false;
    private Rigidbody rb;

    private void Awake()
    {
        bulletCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    public void Initialize(PortalGun gun, bool blue)
    {
        portalGun = gun;
        isBlue = blue;
    }

    private void FixedUpdate()
    {
        if (rb == null || hasPlacedPortal) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, rb.linearVelocity.normalized, out hit,
                            rb.linearVelocity.magnitude * Time.fixedDeltaTime))
        {
            HandleRaycastHit(hit.point, hit.normal, hit.collider);
        }
    }

    private void HandleRaycastHit(Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        Debug.Log($"Raycast detected hit on {hitCollider.name}");

        if (!bulletCollider.enabled) return;

        // Only allow portals on valid surfaces
        string surfaceName = hitCollider.gameObject.name;
        if (!surfaceName.Contains("Wall") && !surfaceName.Contains("Terrain")) return;

        if (hasPlacedPortal) return;
        hasPlacedPortal = true;

        Vector3 portalPosition = hitPoint;
        Quaternion portalRotation = Quaternion.LookRotation(hitNormal);

        if (portalGun != null)
        {
            portalGun.PlacePortal(portalPosition, portalRotation, isBlue);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPlacedPortal) return;
        Debug.Log($"Bullet collided with {collision.collider.name} at {Time.time}");

        ContactPoint contact = collision.contacts[0];
        HandleRaycastHit(contact.point, contact.normal, collision.collider);
    }
}
