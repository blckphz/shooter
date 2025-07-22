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
    public Image dashcd;
    public GameObject gameOverScreen;

    [Header("Smoothing Settings")]
    [Tooltip("How fast velocity moves toward target when you ARE providing input.")]
    public float moveSmoothing = 10f;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Grappling Hook Pulling")]
    public float hookPullSpeed = 20f;      // Pull speed when hooked
    public bool isHooked = false;          // Are we currently being pulled?
    public Vector3 hookPoint;              // Target hook point
    public grapplingHook activeHook;       // Current grappling hook reference

    [Header("Audio")]
    private AudioSource audioSource;

    // --- Private state ---
    private Rigidbody rb;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;
    private Vector3 moveInput;
    private bool isOnIce = false;
    private float iceSpeedMultiplier = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (dashcd == null)
        {
            dashcd = GameObject.Find("Dashes")?.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (PlayerHealth.Health <= 0 || (gameOverScreen != null && gameOverScreen.activeSelf))
            return;

        // Timers
        if (dashCooldownTimer > 0f) dashCooldownTimer -= Time.deltaTime;
        if (dashTimer > 0f) dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f && isDashing)
            isDashing = false;

        UpdateDashCooldownUI();

        // Only capture input when not hooked
        if (!isHooked)
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f)
            {
                if (cameraTransform == null)
                {
                    Debug.LogWarning("CameraTransform not assigned to PlayerMove script.");
                    return;
                }

                Vector3 dashDir = cameraTransform.forward.normalized;
                if (dashDir != Vector3.zero)
                    StartDash(dashDir);
            }
        }
        else
        {
            moveInput = Vector3.zero; // Lock movement while hooked
        }
    }

    void FixedUpdate()
    {
        if (isHooked)
        {
            PullToHookPoint();
            return; // Skip normal movement while hooked
        }

        if (isDashing)
        {
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
            }
        }

        ApplyExtraGravity();
    }

    void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = direction.normalized;
    }

    void UpdateDashCooldownUI()
    {
        if (dashcd == null) return;
        dashcd.fillAmount = 1f - Mathf.Clamp01(dashCooldownTimer / dashCooldown);
    }

    public void ResetDash()
    {
        dashCooldownTimer = 0f;
        dashTimer = 0f;
        isDashing = false;
        UpdateDashCooldownUI();
    }

    void ApplyExtraGravity()
    {
        if (rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Physics.gravity * (fallMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    public void SetIceState(bool state, float multiplier)
    {
        isOnIce = state;
        iceSpeedMultiplier = multiplier;
    }

    // Called by grappling hook to start pulling player
    public void StartHookPull(Vector3 point)
    {
        isHooked = true;
        hookPoint = point;
        isDashing = false;

    }

    // Called to stop pulling player (e.g., when hook detaches)
    public void StopHookPull()
    {
        isHooked = false;

            audioSource.Stop();
    }

    // Pull player toward hook point smoothly
    void PullToHookPoint()
    {
        Vector3 direction = hookPoint - transform.position;
        float distance = direction.magnitude;

        if (distance > 0.5f)
        {

            activeHook.grapplingAudio.Play();

            direction.Normalize();
            Vector3 pullVelocity = direction * hookPullSpeed;

            // Pull in all directions (including vertical)
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, pullVelocity, Time.fixedDeltaTime * 10f);
        }
        else
        {
            // Close enough to hook point - stop pulling
            StopHookPull();
            rb.linearVelocity = Vector3.zero;
        }
    }
}
