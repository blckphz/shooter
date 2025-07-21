using UnityEngine;
using Pathfinding;
using System.Collections;

public class enemyMove : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float nextWaypointDistance = 1f;
    public float floatHeight = 1.5f;

    [Header("Grappling Status")]
    public bool grapped = false;
    public Color grappedColor = Color.red;
    public float boostBonus = 5f;
    public int damageOnHit = 20;
    public float grappedDuration = 0.5f;

    private bool hookAttached = false;
    private Coroutine ungrappleCoroutine;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody rb;
    private Color originalColor;
    private Renderer rend;

    public grapplingHook currentHook; // Reference to the attached grappling hook

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
        CheckForHookChild();

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

                // Destroy hook when damaged
                if (currentHook != null)
                {
                    Destroy(currentHook.gameObject);
                    currentHook = null;
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

    public void OnHookAttach(grapplingHook hook)
    {
        hookAttached = true;
        grapped = true;

        currentHook = hook;

        if (ungrappleCoroutine != null)
        {
            StopCoroutine(ungrappleCoroutine);
            ungrappleCoroutine = null;
        }

        Debug.Log("[EnemyMove] Enemy hooked!");
    }

    public void OnHookDetach()
    {
        if (currentHook != null)
        {
            currentHook = null;
        }

        if (ungrappleCoroutine == null)
            ungrappleCoroutine = StartCoroutine(RemoveGrappleAfterDelay(grappedDuration));
    }

    private IEnumerator RemoveGrappleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!HasHookChild())
        {
            hookAttached = false;
            grapped = false;
            Debug.Log("[EnemyMove] No hook child found, removing grapple status.");
        }

        ungrappleCoroutine = null;
    }

    private void CheckForHookChild()
    {
        if (!HasHookChild() && hookAttached)
        {
            if (ungrappleCoroutine == null)
                ungrappleCoroutine = StartCoroutine(RemoveGrappleAfterDelay(0.5f));
        }
    }

    private bool HasHookChild()
    {
        return transform.Find("hook(Clone)") != null;
    }
}
