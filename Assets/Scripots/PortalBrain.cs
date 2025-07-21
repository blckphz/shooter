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

        // Teleport position with offset
        rb.position = targetPortal.position + targetPortal.forward * 1.0f;

        // Rotate velocity to target portal's forward direction
        rb.linearVelocity = targetPortal.forward * rb.linearVelocity.magnitude;

        yield return new WaitForSeconds(teleportCooldown);

        canTeleport = true;
    }
}
