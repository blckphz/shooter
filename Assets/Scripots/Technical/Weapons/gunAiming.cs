using UnityEngine;

public class gunAiming : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform cameraTransform;
    public Transform gunPivot;
    public GameObject gameOverScreen;

    public float zoomedFOV = 30f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;
    public GunBehaviour gunBehaviour;

    public GameObject someOtherUI;
    public ColliderGameOver groundCheck;

    private float xRotation = 0f;
    private Camera cam;
    public bool isLocked;

    void Start()
    {
        groundCheck = GameObject.Find("Terrain").GetComponent<ColliderGameOver>();
        LockCursor(true);
        cam = Camera.main;
        if (cam != null)
        {
            normalFOV = cam.fieldOfView;
        }
    }

    void Update()
    {
        if (!isLocked)
        {
            return;
        }

        if (PlayerHealth.Health <= 0)
        {
            LockCursor(false);
            ResetTimeScale();
            return;
        }
        else
        {
            LockCursor(true);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        gunPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        bool isRightMouseHeld = Input.GetMouseButton(1);

        if (gunBehaviour.currentGunIndex == 0)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,
                isRightMouseHeld ? zoomedFOV : normalFOV,
                Time.deltaTime * zoomSpeed);
        }
        else if (cam != null)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        }

        bool isPistolEquipped = gunBehaviour.currentGunIndex == 0;
        bool isAirborne = !groundCheck.isGrounded;

        if (isPistolEquipped && isAirborne && isRightMouseHeld)
        {
            SetTimeScale(0.3f);
        }
        else
        {
            ResetTimeScale();
        }
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * scale;
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void LockCursor(bool shouldLock)
    {
        isLocked = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}