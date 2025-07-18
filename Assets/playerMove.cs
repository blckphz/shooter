using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("UI")]
    public Image dashcd;
    public GameObject gameOverScreen; // Assign in Inspector

    private Rigidbody rb;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogError("Rigidbody component missing from player!");
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Helps smooth physics
    }

    void Update()
    {
        // Block movement if dead or Game Over screen active
        if (PlayerHealth.Health <= 0 || (gameOverScreen != null && gameOverScreen.activeSelf))
            return;

        // Handle cooldown timers
        if (dashCooldownTimer > 0f) dashCooldownTimer -= Time.deltaTime;
        if (dashTimer > 0f) dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f && isDashing) isDashing = false;

        fillDashCooldownUI();

        // Store input (do not move in Update)
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        // Dash input
        if (Input.GetKey(KeyCode.LeftShift) && dashCooldownTimer <= 0f && moveInput != Vector3.zero)
        {
            StartDash(moveInput);
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // Smooth dash (instead of abrupt velocity set)
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dashDirection * dashSpeed, Time.fixedDeltaTime * 15f);
        }
        else
        {
            // Smooth normal movement
            Vector3 targetVelocity = transform.TransformDirection(moveInput) * moveSpeed;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z), Time.fixedDeltaTime * 10f);
        }
    }

    void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = transform.TransformDirection(direction);
    }

    void fillDashCooldownUI()
    {
        float fillAmountForImage = Mathf.Clamp01(dashCooldownTimer / dashCooldown);
        dashcd.fillAmount = fillAmountForImage;
    }

    public void resetDash()
    {
        dashCooldownTimer = 0f;
    }
}
