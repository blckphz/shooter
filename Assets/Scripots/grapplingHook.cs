using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    public LineRenderer line;
    public Transform player;
    public float pullSpeed = 20f;
    public float maxDistance = 50f;
    public float idleTimeBeforeDetach = 1f;
    public float velocityThreshold = 0.1f;

    private bool isAttached = false;
    private Rigidbody rb;
    private Vector3 hitPoint;
    private float idleTimer = 0f;
    private enemyMove grabbedEnemy; // Reference to enemy

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    void Update()
    {
        if (player == null) return;

        line.SetPosition(0, player.position);
        line.SetPosition(1, transform.position);

        if (Input.GetMouseButtonDown(1) || Vector3.Distance(player.position, transform.position) > maxDistance)
        {
            Debug.Log("[GrapplingHook] Manual detach triggered.");
            Detach();
        }

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
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            hitPoint = transform.position;
            isAttached = true;

            // Parent hook to enemy so it moves with them
            transform.SetParent(collision.gameObject.transform);

            grabbedEnemy = collision.gameObject.GetComponent<enemyMove>();
            if (grabbedEnemy != null)
            {
                grabbedEnemy.OnHookAttach();
                Debug.Log("[GrapplingHook] Hook attached to enemy: " + collision.gameObject.name);
            }
        }
    }

    void PullPlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 direction = (hitPoint - player.position).normalized;
            playerRb.linearVelocity = direction * pullSpeed;
        }

        if (Vector3.Distance(player.position, hitPoint) < 2f)
        {
            Debug.Log("[GrapplingHook] Player reached hook point, detaching.");
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
                    Debug.Log("[GrapplingHook] Hook idle too long, detaching.");
                    Detach();
                }
            }
            else
            {
                idleTimer = 0f;
            }
        }
    }

    void Detach()
    {
        if (grabbedEnemy != null)
        {
            grabbedEnemy.OnHookDetach();
            Debug.Log("[GrapplingHook] Hook detached from enemy.");
            grabbedEnemy = null;
        }

        Destroy(gameObject);
    }
}
