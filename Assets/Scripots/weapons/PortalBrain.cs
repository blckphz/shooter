using UnityEngine;
using System.Collections;

public class PortalBrain : MonoBehaviour
{
    public static PortalBrain Instance { get; private set; }

    public Transform bluePortal;
    public Transform orangePortal;

    public bool canTeleport = true;
    private float teleportCooldown = 0.2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPortals(Transform blue, Transform orange)
    {
        bluePortal = blue;
        orangePortal = orange;
    }

    public void TryTeleport(Rigidbody rb, Transform enteredPortal)
    {
        if (!canTeleport || bluePortal == null || orangePortal == null) return;

        Transform targetPortal = (enteredPortal == bluePortal) ? orangePortal : bluePortal;
        if (targetPortal == null) return;

        StartCoroutine(Teleport(rb, targetPortal));
    }


private IEnumerator Teleport(Rigidbody rb, Transform targetPortal)
    {
        canTeleport = false;

        // Determine offset size based on object bounds
        float objectOffset = 0.2f; // fallback offset
        Collider col = rb.GetComponent<Collider>();
        if (col != null)
        {
            objectOffset = col.bounds.extents.magnitude * 0.5f;
        }

        Vector3 targetPosition = targetPortal.position + targetPortal.forward * 1.0f + targetPortal.up * objectOffset;

        // Log debug information
        Debug.Log($"[Teleport] Portal: {targetPortal.name}");
        Debug.Log($"[Teleport] Portal Forward: {targetPortal.forward}, Portal Up: {targetPortal.up}");
        Debug.Log($"[Teleport] Offset Magnitude: {objectOffset}");
        Debug.Log($"[Teleport] Final Target Position: {targetPosition}");

        // Apply teleport
        rb.position = targetPosition;

        // Adjust velocity to match target portal direction
        rb.linearVelocity = targetPortal.forward * rb.linearVelocity.magnitude;
        Debug.Log($"[Teleport] New Velocity: {rb.linearVelocity}");

        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

}
