using UnityEngine;
using System.Collections;

public class grapplingHook : MonoBehaviour
{
    public LineRenderer line;
    public Transform player;
    public float pullSpeed = 20f;
    public float maxDistance = 50f;

    private bool isAttached = false;
    private Rigidbody rb;
    private Vector3 hitPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    void Update()
    {
        if (player == null) return;

        // Update rope positions
        line.SetPosition(0, player.position);
        line.SetPosition(1, transform.position);

        // Detach if player presses right-click or rope too long
        if (Input.GetMouseButtonDown(1) || Vector3.Distance(player.position, transform.position) > maxDistance)
        {
            Detach();
        }

        // If attached, pull player
        if (isAttached)
        {
            PullPlayer();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isAttached && collision.gameObject.CompareTag("Enemy"))
        {
            rb.linearVelocity = Vector3.zero; // Stop hook
            rb.isKinematic = true;      // Freeze
            hitPoint = transform.position;
            isAttached = true;
        }
    }

    void PullPlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 direction = (hitPoint - player.position).normalized;
            playerRb.linearVelocity = direction * pullSpeed; // Directly set velocity
        }

        if (Vector3.Distance(player.position, hitPoint) < 2f)
        {
            Detach();
        }
    }


    void Detach()
    {
        Destroy(gameObject); // Destroy hook
    }
}
