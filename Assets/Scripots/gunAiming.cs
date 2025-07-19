using UnityEngine;

public class gunAiming : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform cameraTransform;
    public Transform gunPivot;
    public GameObject gameOverScreen;

    // Zoom parameters
    public float zoomedFOV = 30f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;
    public GunBehaviour gunBehaviour;


    private float xRotation = 0f;

    private Camera cam;


    void Start()
    {
        LockCursor(true);
        cam = Camera.main; // get main camera reference once
        if (cam != null)
        {
            normalFOV = cam.fieldOfView; // store normal FOV at start
        }
    }

    void Update()
    {
        // Stop aiming if dead or Game Over screen active
        if (PlayerHealth.Health <= 0 || (gameOverScreen != null && gameOverScreen.activeSelf))
        {
            LockCursor(false);
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

        // Right-click aim zoom only if NOT using the portal gun
        if (gunBehaviour.currentGunIndex == 0)
        {
            if (Input.GetMouseButton(1)) // right mouse button held
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomedFOV, Time.deltaTime * zoomSpeed);
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
            }
        }
        else if (cam != null)
        {
            // If using portal gun, reset FOV to normal just in case
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        }
    }

    void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
