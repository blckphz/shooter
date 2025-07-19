using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    public LineRenderer line;
    public Transform player;
    public float pullSpeed = 20f;
    public float maxDistance = 50f;
    public float idleTimeBeforeDetach = 1f; // Time in seconds before destroying if idle
    public float velocityThreshold = 0.1f;  // Threshold to consider as "not moving"

    private bool isAttached = false;
    private Rigidbody rb;
    private Vector3 hitPoint;
    private float idleTimer = 0f;

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
            CheckIdle();
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

    void CheckIdle()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            bool playerStill = playerRb.linearVelocity.magnitude < velocityThreshold;
            bool hookStill = rb.linearVelocity.magnitude < velocityThreshold;

            if (playerStill && hookStill)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTimeBeforeDetach)
                {
                    Detach();
                }
            }
            else
            {
                idleTimer = 0f; // Reset timer if movement detected
            }
        }
    }

    void Detach()
    {
        Destroy(gameObject); // Destroy hook
    }
}
