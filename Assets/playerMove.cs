using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Gravity Settings")]
    [Tooltip("Multiplier applied while falling (Y velocity < 0). 1 = normal gravity.")]
    public float fallMultiplier = 2.5f;

    [Header("UI")]
    public Image dashcd;              // Cooldown radial (optional)
    public GameObject gameOverScreen; // Assign in Inspector

    [Header("Smoothing Settings")]
    [Tooltip("How fast velocity moves toward target when you ARE providing input.")]
    public float moveSmoothing = 10f;

    // --- Private state ---
    Rigidbody rb;
    bool isDashing = false;
    float dashTimer = 0f;
    float dashCooldownTimer = 0f;
    Vector3 dashDirection;
    Vector3 moveInput;
    bool isOnIce = false;
    float iceSpeedMultiplier = 1f; // default no multiplier

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Optional safety settings:
        // rb.freezeRotation = true; // Uncomment to prevent tipping over.
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoother visual motion.
    }

    void Update()
    {
        // Block input if dead or Game Over active
        if (PlayerHealth.Health <= 0 || (gameOverScreen != null && gameOverScreen.activeSelf))
            return;

        // Update timers
        if (dashCooldownTimer > 0f) dashCooldownTimer -= Time.deltaTime;
        if (dashTimer > 0f) dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f && isDashing)
            isDashing = false;

        UpdateDashCooldownUI();

        // Capture raw input in Update (better responsiveness)
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && moveInput != Vector3.zero)
        {
            StartDash(moveInput);
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // Use full dash velocity including Y (no override)
            rb.linearVelocity = dashDirection * dashSpeed;
        }
        else
        {
            if (moveInput.magnitude > 0.01f)
            {
                float currentSpeed = isOnIce ? moveSpeed * iceSpeedMultiplier : moveSpeed;
                Vector3 targetVelocity = transform.TransformDirection(moveInput) * currentSpeed;
                Vector3 current = rb.linearVelocity;
                Vector3 newVel = Vector3.Lerp(
                    current,
                    new Vector3(targetVelocity.x, current.y, targetVelocity.z),
                    Time.fixedDeltaTime * moveSmoothing
                );
                rb.linearVelocity = newVel;
            }
            else
            {
                if (!isOnIce)
                {
                    rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                }
                // else: on ice, let sliding happen naturally
            }
        }

        ApplyExtraGravity();
    }


    void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        // Store world‑space direction
        dashDirection = transform.TransformDirection(direction).normalized;
    }

    void UpdateDashCooldownUI()
    {
        if (dashcd == null) return;

        // Common UX: full when ready
        float fill = 1f - Mathf.Clamp01(dashCooldownTimer / dashCooldown);
        dashcd.fillAmount = fill;
    }

    /// <summary>Instantly makes dash available again (e.g., power‑up).</summary>
    public void ResetDash()
    {
        dashCooldownTimer = 0f;
        dashTimer = 0f;
        isDashing = false;
        UpdateDashCooldownUI();
    }

    // Extra gravity for faster falling
    void ApplyExtraGravity()
    {
        // Only apply when falling
        if (rb.linearVelocity.y < 0f)
        {
            // Add the "extra" portion of gravity: (fallMultiplier - 1) * Physics.gravity
            rb.AddForce(Physics.gravity * (fallMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    // Called from SlideBox to inform player they are on ice or not, and pass speed multiplier
    public void SetIceState(bool state, float multiplier)
    {
        isOnIce = state;
        iceSpeedMultiplier = multiplier;
    }
}
