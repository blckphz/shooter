using UnityEngine;
using Pathfinding;  // Import A* Pathfinding namespace

public class enemyMove : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float nextWaypointDistance = 1f;
    public float floatHeight = 1.5f;

    [Header("Grappling Status")]
    public bool grapped = false;               // True when grapped
    public Color grappedColor = Color.red;     // Color when grapped
    public float boostBonus = 5f;              // Boost when grapped
    public int damageOnHit = 20;               // Damage to player if not grapped
    public float grappedDuration = 0.5f;       // Duration to stay grapped after hook is destroyed

    public float grappedTimer = 0f;
    private bool hookAttached = false;         // Tracks actual hook attachment

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody rb;
    private Color originalColor;
    private Renderer rend;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("[EnemyMove] No GameObject with tag 'Player' found in the scene!");
        }

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;

        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (player == null || grapped) return;

        if (seeker.IsDone())
            seeker.StartPath(rb.position, player.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        // --- Handle Grapple Timer ---
        if (!hookAttached && grappedTimer > 0f)
        {
            grappedTimer -= Time.fixedDeltaTime;
            if (grappedTimer <= 0f)
            {
                grapped = false;
                Debug.Log("[EnemyMove] Enemy is no longer grapped (timer ended).");
            }
        }

        // --- If Grapped, Stop Movement ---
        if (grapped)
        {
            rb.linearVelocity = Vector3.zero;
            if (rend != null) rend.material.color = grappedColor;
            return;
        }
        else if (rend != null)
        {
            rend.material.color = originalColor;
        }

        if (player == null || path == null || currentWaypoint >= path.vectorPath.Count)
            return;

        // --- Path Movement ---
        Vector3 currentPosition = rb.position;
        Vector3 targetPosition = path.vectorPath[currentWaypoint];

        targetPosition.y = floatHeight;
        currentPosition.y = floatHeight;

        Vector3 direction = (targetPosition - currentPosition).normalized;
        Vector3 moveAmount = direction * speed * Time.fixedDeltaTime;

        Vector3 newPosition = currentPosition + moveAmount;
        newPosition.y = floatHeight;

        rb.MovePosition(newPosition);

        float distance = Vector3.Distance(
            new Vector3(currentPosition.x, 0, currentPosition.z),
            new Vector3(targetPosition.x, 0, targetPosition.z));

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();

            if (grapped)
            {
                enemyStats stats = GetComponent<enemyStats>();
                if (stats != null)
                {
                    stats.TakeDamage(damageOnHit);
                }

                if (playerRb != null)
                {
                    Vector3 velocity = playerRb.linearVelocity;
                    velocity.y = boostBonus;
                    playerRb.linearVelocity = velocity;
                }
            }
            else
            {
                PlayerHealth.Health -= damageOnHit;
                Debug.Log("[EnemyMove] Player hit by enemy! Health: " + PlayerHealth.Health);

                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.DestroyActiveGrapplingHook();
                }

                if (PlayerHealth.Health <= 0)
                {
                    if (playerHealth != null)
                        playerHealth.SendMessage("ShowGameOver");
                }
            }
        }
    }

    // --- Called by Grappling Hook on Attach ---
    public void OnHookAttach()
    {
        hookAttached = true;
        grapped = true;
        grappedTimer = 0f; // Reset timer while attached
        Debug.Log("[EnemyMove] Enemy hooked!");
    }

    // --- Called by Grappling Hook on Detach ---
    public void OnHookDetach()
    {
        hookAttached = false;
        grappedTimer = grappedDuration; // Start countdown
        Debug.Log("[EnemyMove] Hook detached, enemy will un-grap after " + grappedDuration + " seconds.");
    }

    // --- Check if the hook is still attached ---
    public bool IsHookAttached()
    {
        return hookAttached;
    }
}
