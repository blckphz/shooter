using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    public Portal linkedPortal;  // Link to the other portal

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            PortalBrain.Instance.TryTeleport(rb, transform);
            Debug.Log("playerTele");
        }
    }
}
