using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
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
